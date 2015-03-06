﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Hapil;
using NWheels.Extensions;

namespace NWheels.Logging
{
    public abstract class LogNode
    {
        private readonly string _messageId;
        private LogContentTypes _contentTypes;
        private LogLevel _level;
        private long _millisecondsTimestamp;
        private IThreadLog _threadLog = null;
        private LogNode _nextSibling = null;
        private string _formattedSingleLineText = null;
        private string _formattedFullDetailsText = null;
        private string _formattedNameValuePairsText = null;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected LogNode(string messageId, LogContentTypes contentTypes, LogLevel initialLevel)
        {
            _messageId = messageId;
            _contentTypes = contentTypes;
            _level = initialLevel;
            _millisecondsTimestamp = -1;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public virtual ThreadLogSnapshot.LogNodeSnapshot TakeSnapshot()
        {
            return new ThreadLogSnapshot.LogNodeSnapshot {
                MillisecondsTimestamp = _millisecondsTimestamp,
                Level = _level,
                ContentTypes = _contentTypes,
                SingleLineText = this.SingleLineText,
                FullDetailsText = this.FullDetailsText,
                ExceptionTypeName = (this.Exception != null ? this.Exception.GetType().FullName : null)
            };
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string MessageId
        {
            get
            {
                return _messageId;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public long MillisecondsTimestamp
        {
            get
            {
                return _millisecondsTimestamp;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public LogLevel Level
        {
            get
            {
                return _level;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public LogContentTypes ContentTypes
        {
            get
            {
                return _contentTypes;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string SingleLineText
        {
            get
            {
                if ( _formattedSingleLineText == null )
                {
                    _formattedSingleLineText = FormatSingleLineText();
                }

                return _formattedSingleLineText;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string FullDetailsText
        {
            get
            {
                if ( _formattedFullDetailsText == null )
                {
                    _formattedFullDetailsText = FormatFullDetailsText();
                }

                return _formattedFullDetailsText;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string NameValuePairsText
        {
            get
            {
                if ( _formattedNameValuePairsText == null )
                {
                    _formattedNameValuePairsText = FormatNameValuePairsText(delimiter: " ");
                }

                return _formattedNameValuePairsText;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public string ExceptionTypeName
        {
            get
            {
                return (this.Exception != null ? this.Exception.GetType().FullName : null);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public LogNode NextSibling
        {
            get
            {
                return _nextSibling;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public virtual Exception Exception
        {
            get
            {
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public virtual ThreadTaskType TaskType
        {
            get
            {
                return ThreadTaskType.Unspecified;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal virtual void AttachToThreadLog(IThreadLog thread, ActivityLogNode parent)
        {
            _threadLog = thread;
            _millisecondsTimestamp = thread.ElapsedThreadMilliseconds;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal void AttachNextSibling(LogNode sibling)
        {
            if ( _nextSibling == null )
            {
                _nextSibling = sibling;
            }
            else
            {
                throw new InvalidOperationException("This log node already has attached sibling node");
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected string MessageIdToText()
        {
            return LogMessageHelper.GetTextFromMessageId(_messageId);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string FormatSingleLineText()
        {
            return MessageIdToText();
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string FormatFullDetailsText()
        {
            return FormatNameValuePairsText(delimiter: System.Environment.NewLine);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected virtual string FormatNameValuePairsText(string delimiter)
        {
            var node = _threadLog.Node;

            var baseValues =
                _threadLog.ThreadStartedAtUtc.AddMilliseconds(_millisecondsTimestamp).ToString("yyyy-MM-dd HH:mm:ss.fff") + delimiter +
                FormatNameValuePair("app", node.ApplicationName) + delimiter +
                FormatNameValuePair("node", node.NodeName) + delimiter +
                FormatNameValuePair("instance", node.InstanceId) + delimiter +
                FormatNameValuePair("env", node.EnvironmentName) + delimiter +
                FormatNameValuePair("message", _messageId) + delimiter +
                FormatNameValuePair("level", _level.ToString()) + delimiter +
                FormatNameValuePair("logid", _threadLog.LogId.ToString("N"));

            if ( this.Exception != null )
            {
                baseValues += delimiter + FormatNameValuePair("exception", this.Exception.GetType().FullName);
            }

            return baseValues;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected void BubbleLogLevelFrom(LogLevel subNodeLevel)
        {
            if ( subNodeLevel > _level )
            {
                _level = subNodeLevel;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        protected void BubbleContentTypesFrom(LogContentTypes subNodeContentTypes)
        {
            _contentTypes |= subNodeContentTypes;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        internal IThreadLog ThreadLog
        {
            get
            {
                return _threadLog;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public static string FormatNameValuePair(string name, string value)
        {
            name = name.TruncateAt(50);
            value = (value != null ? value.TruncateAt(255).Replace('"', '\'') : "null");

            if ( value.Any(c => char.IsWhiteSpace(c) || c == '=') )
            {
                value = "\"" + value + "\"";
            }

            return (name + "=" + value);
        }
    }
}
