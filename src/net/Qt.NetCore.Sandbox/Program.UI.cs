﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Qt.NetCore.Qml;
using Qt.NetCore.Types;

namespace Qt.NetCore.Sandbox
{
    class Program
    {
        [Signal("testSignal", NetVariantType.String)]
        public class TestQmlImport
        {
            readonly AnotherType _anotherType = new AnotherType();
            
            public AnotherType GetSharedInstance()
            {
                return _anotherType;
            }
        }

        [Signal("testSignal", NetVariantType.String)]
        public class AnotherType
        {
            
        }

        public class InstanceType
        {
            public InstanceType()
            {

            }
        }

        public class TestQmlInstanceHandling
        {
            private InstanceType _instanceType;
            private WeakReference<InstanceType> _weakInstanceTypeRef;

            public int State { get; set; } = 0;

            public TestQmlInstanceHandling()
            {
                _instanceType = new InstanceType();
                _weakInstanceTypeRef = new WeakReference<InstanceType>(_instanceType);
            }

            public InstanceType GetInstance()
            {
                return _instanceType;
            }

            public void DeleteInstances()
            {
                _instanceType = null;
            }

            public bool IsInstanceAlive()
            {
                return _weakInstanceTypeRef.TryGetTarget(out var _);
            }
        }
        
        static int Main()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    GC.Collect(GC.MaxGeneration);
                }
            });
            using (var app = new QGuiApplication())
            {
                using (var engine = new QQmlApplicationEngine())
                {
                    var type = NetTypeManager.GetTypeInfo<TestQmlImport>();
                    
                    QQmlApplicationEngine.RegisterType<TestQmlImport>("test");
                    QQmlApplicationEngine.RegisterType<TestQmlInstanceHandling>("testInstances");

                    engine.Load("main.qml");

                    return app.Exec();
                }
            }
        }
    }
}
