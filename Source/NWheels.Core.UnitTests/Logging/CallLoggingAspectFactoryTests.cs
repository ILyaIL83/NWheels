﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hapil;
using Hapil.Testing.NUnit;
using NUnit.Framework;
using NWheels.Core.Logging;
using NWheels.Logging;
using NWheels.Testing;

namespace NWheels.Core.UnitTests.Logging
{
    [TestFixture]
    public class CallLoggingAspectFactoryTests : NUnitEmittedTypesTestBase
    {
        private ConventionObjectFactory _factory;
        private TestThreadLogAppender _logAppender;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _factory = new CallLoggingAspectFactory(base.Module);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [SetUp]
        public void SetUp()
        {
            _logAppender = new TestThreadLogAppender(new TestFramework(base.Module));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void CanCreateAspectObject()
        {
            //-- Arrange

            var realComponent = new RealComponent(_logAppender);

            //-- Act
            
            var decoratedComponent = _factory.CreateInstanceOf<ITestComponent>().UsingConstructor<object, IThreadLogAppender>(realComponent, _logAppender);

            //-- Assert

            Assert.That(decoratedComponent, Is.Not.Null);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void CanLogVoidCallWithNoParameters()
        {
            //-- Arrange

            var realComponent = new RealComponent(_logAppender);
            var decoratedComponent = _factory.CreateInstanceOf<ITestComponent>().UsingConstructor<object, IThreadLogAppender>(realComponent, _logAppender);

            //-- Act

            decoratedComponent.ThisIsMyVoidMethod();

            //-- Assert

            Assert.That(_logAppender.GetLogStrings(), Is.EqualTo(new[] {
                "TestComponent.ThisIsMyVoidMethod", 
                "BACKEND:ThisIsMyVoidMethod",
            }));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void CanLogVoidCallWithParameters()
        {
            //-- Arrange

            var realComponent = new RealComponent(_logAppender);
            var decoratedComponent = _factory.CreateInstanceOf<ITestComponent>().UsingConstructor<object, IThreadLogAppender>(realComponent, _logAppender);

            //-- Act

            decoratedComponent.ThisIsMyVoidMethodWithParameters(num: 123, str: "ABC");

            //-- Assert

            Assert.That(_logAppender.GetLogStrings(), Is.EqualTo(new[] {
                "TestComponent.ThisIsMyVoidMethodWithParameters", 
                "BACKEND:ThisIsMyVoidMethod",
            }));

            var nameValuePairs = _logAppender.GetLog()[0].NameValuePairs.Where(nvp => !nvp.IsBaseValue()).ToArray();

            Assert.That(nameValuePairs.Length, Is.EqualTo(2));
            Assert.That(nameValuePairs[0].FormatLogString(), Is.EqualTo("num=123"));
            Assert.That(nameValuePairs[1].FormatLogString(), Is.EqualTo("str=ABC"));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        [Test]
        public void CanLogFunctionCallWithParameters()
        {
            //-- Arrange

            var realComponent = new RealComponent(_logAppender);
            var decoratedComponent = _factory.CreateInstanceOf<ITestComponent>().UsingConstructor<object, IThreadLogAppender>(realComponent, _logAppender);

            //-- Act

            var returnValue = decoratedComponent.ThisIsMyFunction(987, "XYZ");

            //-- Assert

            Assert.That(_logAppender.GetLogStrings(), Is.EqualTo(new[] {
                "TestComponent.ThisIsMyFunction", 
                "BACKEND:ThisIsMyFunction",
                "Logging call outputs", 
            }));

            var inputNameValuePairs = _logAppender.GetLog()[0].NameValuePairs.Where(nvp => !nvp.IsBaseValue()).ToArray();

            Assert.That(inputNameValuePairs.Length, Is.EqualTo(2));
            Assert.That(inputNameValuePairs[0].FormatLogString(), Is.EqualTo("num=987"));
            Assert.That(inputNameValuePairs[1].FormatLogString(), Is.EqualTo("str=XYZ"));

            var outputNameValuePairs = _logAppender.GetLog()[2].NameValuePairs.Where(nvp => !nvp.IsBaseValue()).ToArray();

            Assert.That(outputNameValuePairs.Length, Is.EqualTo(1));
            Assert.That(outputNameValuePairs[0].FormatLogString(), Is.EqualTo("return=ABC"));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public interface ITestComponent
        {
            void ThisIsMyVoidMethod();
            void ThisIsMyVoidMethodWithParameters(int num, string str);
            string ThisIsMyFunction(int num, string str);
            DateTime ThisIsMyMethodWithPrimitiveValues(TimeSpan time, DayOfWeek day);
            MyReplyObject ThisIsMyMethodWithLoggableObjects(MyRequestObject request);
            DayOfWeek ThisIsMyMethodWithRefOutParameters(ref int num, out string str);
            XElement ThisIsMyMethodWithXmlSerializableObjects(XElement input);
            object ThisIsMyMethodWithNonLoggableObjects(Stream data);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public class RealComponent : ITestComponent
        {
            private readonly IThreadLogAppender _logAppender;

            //-------------------------------------------------------------------------------------------------------------------------------------------------

            public RealComponent(IThreadLogAppender logAppender)
            {
                _logAppender = logAppender;
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------

            public void ThisIsMyVoidMethod()
            {
                _logAppender.AppendLogNode(new NameValuePairLogNode("!BACKEND:ThisIsMyVoidMethod", LogLevel.Debug, exception: null));
            }
            public void ThisIsMyVoidMethodWithParameters(int num, string str)
            {
                _logAppender.AppendLogNode(new NameValuePairLogNode("!BACKEND:ThisIsMyVoidMethod", LogLevel.Debug, exception: null));
            }
            public string ThisIsMyFunction(int num, string str)
            {
                _logAppender.AppendLogNode(new NameValuePairLogNode("!BACKEND:ThisIsMyFunction", LogLevel.Debug, exception: null));
                return "ABC";
            }
            public DateTime ThisIsMyMethodWithPrimitiveValues(TimeSpan time, DayOfWeek day)
            {
                return new DateTime(2010, 10, 10);
            }
            public MyReplyObject ThisIsMyMethodWithLoggableObjects(MyRequestObject request)
            {
                return new MyReplyObject();
            }
            public DayOfWeek ThisIsMyMethodWithRefOutParameters(ref int num, out string str)
            {
                num *= 2;
                str = (num + 1).ToString();
                return DayOfWeek.Tuesday;
            }
            public XElement ThisIsMyMethodWithXmlSerializableObjects(XElement input)
            {
                return new XElement("ABC");
            }
            public object ThisIsMyMethodWithNonLoggableObjects(Stream data)
            {
                return new SomeObject();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public class MyRequestObject
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public class MyReplyObject
        {
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public class SomeObject
        {
        }
    }
}
