﻿using System;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Qml.Net.Internal.Qml;
using Xunit;

namespace Qml.Net.Tests.Qml
{
    public class JsValueTests : BaseQmlTests<JsValueTests.JsTestsQml>
    {
        public class JsTestsQml
        {
            public virtual void Method(INetJsValue value)
            {

            }

            public virtual void MethodWithoutParams()
            {
                
            }
            
            public virtual void MethodWithParameters(string param1, int param2)
            {
                
            }

            public virtual void CallMethodWithJsValue(INetJsValue value, INetJsValue method)
            {
                method.Call(value);
            }

            public class TestObject
            {
                public int CalledCount { get; set; }
                
                public void TestMethod()
                {
                    CalledCount++;
                }
            }
        }

        [Fact]
        public void Can_send_function()
        {
            INetJsValue jsValue = null;
            Mock.Setup(x => x.Method(It.IsAny<INetJsValue>()))
                .Callback(new Action<INetJsValue>(x => jsValue = x));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            test.Method(function(){})
                        }
                    }
                ");

            Mock.Verify(x => x.Method(It.IsAny<INetJsValue>()), Times.Once);
            jsValue.Should().NotBeNull();
            jsValue.IsCallable.Should().BeTrue();
        }
        
        [Fact]
        public void Can_send_non_function()
        {
            INetJsValue jsValue = null;
            Mock.Setup(x => x.Method(It.IsAny<INetJsValue>()))
                .Callback(new Action<INetJsValue>(x => jsValue = x));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            test.Method({})
                        }
                    }
                ");

            Mock.Verify(x => x.Method(It.IsAny<INetJsValue>()), Times.Once);
            jsValue.Should().NotBeNull();
            jsValue.IsCallable.Should().BeFalse();
        }

        [Fact]
        public void Can_invoke_js_callback()
        {
            object result = null;
            Mock.Setup(x => x.Method(It.IsAny<INetJsValue>()))
                .Callback(new Action<INetJsValue>(x =>
                {
                    result = x.Call();
                }));
            Mock.Setup(x => x.MethodWithoutParams());
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            test.Method(function() {
                                test.MethodWithoutParams()
                            })
                        }
                    }
                ");

            Mock.Verify(x => x.Method(It.IsAny<INetJsValue>()), Times.Once);
            Mock.Verify(x => x.MethodWithoutParams(), Times.Once);
            result.Should().BeNull();
        }

        [Fact]
        public void Can_invoke_js_callback_with_parameters()
        {
            object result = null;
            Mock.Setup(x => x.Method(It.IsAny<INetJsValue>()))
                .Callback(new Action<INetJsValue>(x =>
                {
                    result = x.Call("test1", 4);
                }));
            Mock.Setup(x => x.MethodWithParameters("test1", 4));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            test.Method(function(param1, param2) {
                                test.MethodWithParameters(param1, param2)
                            })
                        }
                    }
                ");

            Mock.Verify(x => x.Method(It.IsAny<INetJsValue>()), Times.Once);
            Mock.Verify(x => x.MethodWithParameters("test1", 4), Times.Once);
            result.Should().BeNull();
        }

        [Fact]
        public void Can_invoke_js_callback_with_net_instance()
        {
            var testObject = new JsValueTests.JsTestsQml.TestObject();
            Mock.Setup(x => x.Method(It.IsAny<INetJsValue>()))
                .Callback(new Action<INetJsValue>(x =>
                {
                    x.Call(testObject);
                }));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            test.Method(function(param1) {
                                param1.TestMethod()
                                param1.TestMethod()
                            })
                        }
                    }
                ");

            Mock.Verify(x => x.Method(It.IsAny<INetJsValue>()), Times.Once);
            testObject.CalledCount.Should().Be(2);
        }
        
        [Fact]
        public void Can_pass_js_value_to_callback()
        {
            Mock.CallBase = true;
            Mock.Setup(x => x.MethodWithParameters("test1", 4));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    JsTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            var o = {
                                testProperty1: 'test1',
                                testProperty2: 4
                            }
                            test.CallMethodWithJsValue(o,
                                function(passedIn) {
                                    test.MethodWithParameters(passedIn.testProperty1, passedIn.testProperty2)
                                })
                        }
                    }
                ");

            Mock.Verify(x => x.CallMethodWithJsValue(It.IsAny<INetJsValue>(), It.IsAny<INetJsValue>()), Times.Once);
            Mock.Verify(x => x.MethodWithParameters("test1", 4), Times.Once);
        }
    }
}