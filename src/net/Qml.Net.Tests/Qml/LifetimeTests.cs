﻿using System;
using FluentAssertions;
using Xunit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Qml.Net.Tests.Qml
{
    public class LifetimeTests : BaseQmlTestsWithInstance<LifetimeTests.NetInteropTestQml>
    {
        public class SecondLevelType
        {
            public bool IsSame(SecondLevelType other)
            {
                return ReferenceEquals(this, other);
            }
        }

        public class NetInteropTestQml
        {
            public NetInteropTestQml()
            {
                Parameter = new SecondLevelType();
                Parameter2 = new SecondLevelType();
                _parameterWeakRef = new WeakReference<SecondLevelType>(Parameter);
            }

            public SecondLevelType Parameter { get; set; }
            
            public SecondLevelType Parameter2 { get; set; }

            private readonly WeakReference<SecondLevelType> _parameterWeakRef;

            public void ReleaseNetReferenceParameter()
            {
                Parameter = null;
                Parameter2 = null;
            }
            
            public bool CheckIsParameterAlive()
            {
                GC.Collect(GC.MaxGeneration);
                return _parameterWeakRef.TryGetTarget(out SecondLevelType _);
            }

            public bool TestResult { get; set; }
        }

        [Fact]
        public void Can_handle_multiple_instances_equality_qml()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter
                                var instance2 = test.Parameter

                                test.TestResult = instance1.IsSame(instance2)

                                tc.Quit()
                            }
                        }
                    }
                ");
            ExecApplicationWithTimeout(2000).Should().Be(0);

            Assert.True(Instance.TestResult);
        }

        [Fact]
        public void Can_handle_different_instances_equality_qml()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter;
                                var instance2 = test.Parameter2;

                                test.TestResult = instance1.IsSame(instance2);

                                tc.Quit()
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(2000).Should().Be(0);

            Assert.False(Instance.TestResult);
        }

        [Fact]
        public void Can_handle_instance_deref_of_one_ref_in_qml()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter;
                                var instance2 = test.Parameter;

                                //deref Parameter
                                instance2 = null;
                            
                                gc();

                                test.TestResult = test.CheckIsParameterAlive();

                                tc.Quit()
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(2000).Should().Be(0);

            Assert.True(Instance.TestResult);
        }

        [Fact]
        public void Can_handle_instance_deref_of_all_refs_in_qml()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter;
                                var instance2 = test.Parameter;

                                //deref Parameter
                                instance1 = null;
                                instance2 = null;
                            
                                gc();

                                test.TestResult = test.CheckIsParameterAlive();

                                tc.Quit()
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(2000).Should().Be(0);

            Assert.True(Instance.TestResult);
        }

        [Fact()]
        public void Can_handle_instance_deref_of_all_refs_in_qml_and_net()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        Timer {
                            id: checkAndQuitTimer
                            running: false
                            interval: 1000
			                onTriggered: {
                                test.TestResult = test.CheckIsParameterAlive();

                                tc.Quit()
			                }
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter
                                var instance2 = test.Parameter

                                //deref Parameter
                                instance1 = null
                                instance2 = null
                                test.ReleaseNetReferenceParameter()
                            
                                gc()
                                Net.gcCollect(2)

                                checkAndQuitTimer.running = true
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(3000).Should().Be(0);

            Assert.False(Instance.TestResult);
        }

        [Fact]
        public void Can_handle_qml_reference_keeps_net_object_alive()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        TestContext {
                            id: tc
                        }

                        Timer {
                            id: checkAndQuitTimer
                            running: false
                            interval: 1000
			                onTriggered: {
                                test.TestResult = test.CheckIsParameterAlive();

                                tc.Quit()
			                }
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                var instance1 = test.Parameter

                                test.ReleaseNetReferenceParameter()
                                Net.gcCollect(2)

                                checkAndQuitTimer.running = true
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(3000).Should().Be(0);

            Assert.True(Instance.TestResult);
        }

        [Fact]
        public void Can_handle_deleting_one_qml_ref_does_not_release()
        {
            qmlApplicationEngine.LoadData(@"
                    import QtQuick 2.0
                    import tests 1.0
                    import testContext 1.0

                    Item {
                        property var instanceRef: null
                        TestContext {
                            id: tc
                        }

                        Timer {
                            id: checkAndQuitTimer
                            running: false
                            interval: 1000
			                onTriggered: {
                                test.TestResult = test.CheckIsParameterAlive();

                                tc.Quit()
			                }
                        }

                        NetInteropTestQml {
                            id: test
                            Component.onCompleted: function() {
                                instanceRef = test.Parameter
                                var instance2 = test.Parameter

                                test.ReleaseNetReferenceParameter()

                                //release second QML ref
                                instance2 = null
                                gc()
                                Net.gcCollect(2)

                                checkAndQuitTimer.running = true
                            }
                        }
                    }
                ");

            ExecApplicationWithTimeout(3000).Should().Be(0);

            Assert.True(Instance.TestResult);
        }
    }
}
