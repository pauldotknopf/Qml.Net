﻿using System.Runtime.InteropServices;
using AdvancedDLSupport;
using Qml.Net.Internal;

namespace Qml.Net
{
    public class QQuickStyle
    {
        public static void SetFallbackStyle(string style)
        {
            Interop.QQuickStyle.SetFallbackStyle(style);
        }

        public static void SetStyle(string style)
        {
            Interop.QQuickStyle.SetStyle(style);
        }
    }
    
    internal interface IQQuickStyleInterop
    {
        [NativeSymbol(Entrypoint = "qquickstyle_setFallbackStyle")]
        void SetFallbackStyle([MarshalAs(UnmanagedType.LPWStr), CallerFree]string style);

        [NativeSymbol(Entrypoint = "qquickstyle_setStyle")]
        void SetStyle([MarshalAs(UnmanagedType.LPWStr), CallerFree]string style);
    }
}