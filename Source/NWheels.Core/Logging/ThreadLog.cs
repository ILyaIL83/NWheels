﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NWheels.Hosting;
using NWheels.Logging;

namespace NWheels.Core.Logging
{
    internal class ThreadLog : IThreadLog
    {
        private readonly IThreadRegistry _registry;
        private readonly Guid _logId;
        private readonly Guid _correlationId;
        private readonly INodeConfiguration _node;
        private readonly ThreadTaskType _taskType;
        private readonly DateTime _startedAtUtc;
        private readonly IClock _clock;
        private ActivityLogNode _rootActivity;
        private ActivityLogNode _currentActivity;
        private readonly IThreadLogAnchor _anchor;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ThreadLog(
            IFramework framework, IClock clock, IThreadRegistry registry, IThreadLogAnchor anchor, ThreadTaskType taskType, ActivityLogNode rootActivity)
        {
            _registry = registry;
            _anchor = anchor;
            _taskType = taskType;
            _rootActivity = rootActivity;
            _currentActivity = rootActivity;
            _startedAtUtc = framework.UtcNow;
            _logId = framework.NewGuid();
            _correlationId = _logId;
            _node = framework.CurrentNode;
            _clock = clock;

            _rootActivity.AttachToThreadLog(this, parent: null);
            _registry.ThreadStarted(this);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void AppendNode(LogNode node, bool clearFailure = false)
        {
            node.AttachToThreadLog(this, _currentActivity);
            _currentActivity.AppendChildNode(node, clearFailure);

            var nodeAsActivity = (node as ActivityLogNode);

            if ( nodeAsActivity != null )
            {
                _currentActivity = nodeAsActivity;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public void NotifyActivityClosed(ActivityLogNode activity)
        {
            if ( activity != _currentActivity )
            {
                throw new InvalidOperationException("Cannot close actvity because it is not the current activity at the moment.");
            }

            if ( activity.Parent != null )
            {
                _currentActivity = activity.Parent;
            }
            else
            {
                Close();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ThreadLogSnapshot TakeSnapshot()
        {
            return new ThreadLogSnapshot {
                LogId = _logId,
                CorrelationId = _correlationId,
                StartedAtUtc = _startedAtUtc,
                TaskType = _taskType,
                RootActivity = (ThreadLogSnapshot.ActivityNodeSnapshot)_rootActivity.TakeSnapshot()
            };
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public NWheels.Hosting.INodeConfiguration Node
        {
            get
            {
                return _node; 
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public Guid LogId
        {
            get
            {
                return _logId;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public Guid CorrelationId
        {
            get
            {
                return _correlationId;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ThreadTaskType TaskType
        {
            get
            {
                return _taskType;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public DateTime ThreadStartedAtUtc
        {
            get
            {
                return _startedAtUtc;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public long ElapsedThreadMilliseconds
        {
            get
            {
                return _clock.ElapsedMilliseconds;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ActivityLogNode RootActivity
        {
            get
            {
                return _rootActivity;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public ActivityLogNode CurrentActivity
        {
            get
            {
                return _currentActivity;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        private void Close()
        {
            _anchor.CurrentThreadLog = null;
            _registry.ThreadFinished(this);
        }
    }
}

