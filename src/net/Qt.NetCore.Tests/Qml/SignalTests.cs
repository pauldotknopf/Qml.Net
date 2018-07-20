﻿using System;
using Moq;
using Qt.NetCore.Qml;
using Xunit;

namespace Qt.NetCore.Tests.Qml
{
    public class SignalTests : BaseQmlTests<SignalTests.ObjectTestsQml>
    {
        public class ObjectTestsQml
        {
            public virtual SignalObject GetSignalObject()
            {
                return null;
            }

            public virtual bool SignalRaised { get; set; }

            public virtual void MethodWithArgs(string arg1, int arg2)
            {
                
            }
        }

        [Signal("testSignal")]
        [Signal("testSignalWithArgs", NetVariantType.String, NetVariantType.Int)]
        public class SignalObject
        {
            
        }

        [Fact]
        public void Can_raise_signal_from_qml()
        {
            var signalObject = new SignalObject();
            Mock.Setup(x => x.GetSignalObject()).Returns(signalObject);
            Mock.Setup(x => x.SignalRaised).Returns(false);
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    ObjectTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            var instance = test.GetSignalObject()
                            instance.testSignal.connect(function() {
                                test.SignalRaised = true
                            })
                            instance.testSignal()
                        }
                    }
                ");

            Mock.VerifySet(x => x.SignalRaised = true, Times.Once);
        }
        
        [Fact]
        public void Can_raise_signal_from_qml_with_args()
        {
            var signalObject = new SignalObject();
            Mock.Setup(x => x.GetSignalObject()).Returns(signalObject);
            Mock.Setup(x => x.SignalRaised).Returns(false);
            Mock.Setup(x => x.MethodWithArgs("arg1", 3));
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    ObjectTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            var instance = test.GetSignalObject()
                            instance.testSignalWithArgs.connect(function(arg1, arg2) {
                                test.SignalRaised = true
                                test.MethodWithArgs(arg1, arg2)
                            })
                            instance.testSignalWithArgs('arg1', 3)
                        }
                    }
                ");

            Mock.VerifySet(x => x.SignalRaised = true, Times.Once);
            Mock.Verify(x => x.MethodWithArgs("arg1", 3), Times.Once);
        }
        
        [Fact]
        public void Can_raise_signal_from_qml_different_retrieval_of_net_instance()
        {
            var signalObject = new SignalObject();
            Mock.Setup(x => x.GetSignalObject()).Returns(signalObject);
            Mock.Setup(x => x.SignalRaised).Returns(false);
            
            NetTestHelper.RunQml(qmlApplicationEngine,
                @"
                    import QtQuick 2.0
                    import tests 1.0
                    ObjectTestsQml {
                        id: test
                        Component.onCompleted: function() {
                            var instance1 = test.GetSignalObject()
                            instance1.testSignal.connect(function() {
                                test.SignalRaised = true
                            })
                            var instance2 = test.GetSignalObject()
                            instance2.testSignal()
                        }
                    }
                ");

            Mock.VerifySet(x => x.SignalRaised = true, Times.Once);
        }
    }
}