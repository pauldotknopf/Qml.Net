﻿using System;
using System.Runtime.InteropServices;
using Qml.Net.Internal;

namespace Qml.Net.Internal.Types
{
    internal class NetPropertyInfo : BaseDisposable
    {
        public NetPropertyInfo(NetTypeInfo parentType,
            string name,
            NetTypeInfo returnType,
            bool canRead,
            bool canWrite,
            NetSignalInfo notifySignal)
            : this(Create(parentType,
                name,
                returnType,
                canRead,
                canWrite,
                notifySignal))
        {
            
        }

        public NetPropertyInfo(IntPtr handle, bool ownsHandle = true)
            : base(handle, ownsHandle)
        {
            
        }

        private static IntPtr Create(NetTypeInfo parentType,
            string name,
            NetTypeInfo returnType,
            bool canRead,
            bool canWrite,
            NetSignalInfo notifySignal)
        {
            return Interop.NetPropertyInfo.Create(parentType?.Handle ?? IntPtr.Zero,
                name,
                returnType?.Handle ?? IntPtr.Zero,
                canRead,
                canWrite,
                notifySignal?.Handle ?? IntPtr.Zero);
        }

        public NetTypeInfo ParentType => new NetTypeInfo(Interop.NetPropertyInfo.GetParentType(Handle));

        public string Name => Utilities.ContainerToString(Interop.NetPropertyInfo.GetPropertyName(Handle));
        
        public NetTypeInfo ReturnType => new NetTypeInfo(Interop.NetPropertyInfo.GetReturnType(Handle));

        public bool CanRead => Interop.NetPropertyInfo.GetCanRead(Handle);

        public bool CanWrite => Interop.NetPropertyInfo.GetCanWrite(Handle);

        public NetSignalInfo NotifySignal
        {
            get
            {
                var result = Interop.NetPropertyInfo.GetNotifySignal(Handle);
                return result == IntPtr.Zero ? null : new NetSignalInfo(result);
            }
            set
            {
                Interop.NetPropertyInfo.SetNotifySignal(Handle, value.Handle);
            }
        }
        
        protected override void DisposeUnmanaged(IntPtr ptr)
        {
            Interop.NetPropertyInfo.Destroy(ptr);
        }
    }

    internal class NetPropertyInfoInterop
    {
        [NativeSymbol(Entrypoint = "property_info_create")]
        public CreateDel Create { get; set; }
        public delegate  IntPtr CreateDel(IntPtr parentType,
            [MarshalAs(UnmanagedType.LPWStr)]string methodName,
            IntPtr returnType,
            bool canRead,
            bool canWrite,
            IntPtr notifySignal);
        
        [NativeSymbol(Entrypoint = "property_info_destroy")]
        public DestroyDel Destroy { get; set; }
        public delegate void DestroyDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_getParentType")]
        public GetParentTypeDel GetParentType { get; set; }
        public delegate IntPtr GetParentTypeDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_getPropertyName")]
        public GetPropertyNameDel GetPropertyName { get; set; }
        public delegate IntPtr GetPropertyNameDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_getReturnType")]
        public GetReturnTypeDel GetReturnType { get; set; }
        public delegate IntPtr GetReturnTypeDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_canRead")]
        public GetCanReadDel GetCanRead { get; set; }
        public delegate bool GetCanReadDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_canWrite")]
        public GetCanWriteDel GetCanWrite { get; set; }
        public delegate bool GetCanWriteDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_getNotifySignal")]
        public GetNotifySignalDel GetNotifySignal { get; set; }
        public delegate IntPtr GetNotifySignalDel(IntPtr property);

        [NativeSymbol(Entrypoint = "property_info_setNotifySignal")]
        public SetNotifySignalDel SetNotifySignal { get; set; }
        public delegate void SetNotifySignalDel(IntPtr property, IntPtr signal);
    }
}