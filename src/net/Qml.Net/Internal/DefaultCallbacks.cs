﻿using Qml.Net.Internal.Qml;
using Qml.Net.Internal.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace Qml.Net.Internal
{
    internal class DefaultCallbacks : ICallbacks
    {
        public bool IsTypeValid(string typeName)
        {
            var t = Type.GetType(typeName);
            return t != null;
        }

        public void CreateLazyTypeInfo(IntPtr t)
        {
            using (var type = new NetTypeInfo(t))
            {
                var typeInfo = Type.GetType(type.FullTypeName);
                if (typeInfo == null) throw new InvalidOperationException();

                type.ClassName = typeInfo.Name;

                if (typeInfo == typeof(bool))
                    type.PrefVariantType = NetVariantType.Bool;
                else if (typeInfo == typeof(char))
                    type.PrefVariantType = NetVariantType.Char;
                else if (typeInfo == typeof(int))
                    type.PrefVariantType = NetVariantType.Int;
                else if (typeInfo == typeof(uint))
                    type.PrefVariantType = NetVariantType.UInt;
                else if (typeInfo == typeof(double))
                    type.PrefVariantType = NetVariantType.Double;
                else if (typeInfo == typeof(string))
                    type.PrefVariantType = NetVariantType.String;
                else if (typeInfo == typeof(DateTime))
                    type.PrefVariantType = NetVariantType.DateTime;
                else
                    type.PrefVariantType = NetVariantType.Object;
                
                // All the methods/properties/signals are later populated when needed.
            }
        }

        public void LoadTypeInfo(IntPtr t)
        {
            using (var type = new NetTypeInfo(t))
            {
                var typeInfo = Type.GetType(type.FullTypeName);
                if (typeInfo == null)
                {
                    throw new InvalidOperationException($"Invalid type {type.FullTypeName}");
                }
                
                // Don't grab properties and methods for system-level types.
                if (Helpers.IsPrimitive(typeInfo)) return;

                foreach (var methodInfo in typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (methodInfo.IsGenericMethod) continue; // No generics supported.
                    if (Helpers.IsPrimitive(methodInfo.DeclaringType)) continue;

                    NetTypeInfo returnType = null;

                    if (methodInfo.ReturnParameter != null && methodInfo.ReturnParameter.ParameterType != typeof(void))
                    {
                        returnType =
                            NetTypeManager.GetTypeInfo(methodInfo.ReturnParameter.ParameterType);
                    }

                    var method = new NetMethodInfo(type, methodInfo.Name, returnType);

                    foreach (var parameter in methodInfo.GetParameters())
                    {
                        method.AddParameter(parameter.Name, NetTypeManager.GetTypeInfo(parameter.ParameterType));
                    }

                    type.AddMethod(method);
                }

                var signals = new Dictionary<string, NetSignalInfo>();
                
                foreach (var signalAttribute in typeInfo.GetCustomAttributes().OfType<SignalAttribute>())
                {
                    var signal = new NetSignalInfo(type, signalAttribute.Name);
                    foreach (var parameter in signalAttribute.Parameters)
                    {
                        signal.AddParameter(parameter);
                    }
                    type.AddSignal(signal);
                    signals.Add(signal.Name, signal);
                }
                
                foreach (var propertyInfo in typeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (Helpers.IsPrimitive(propertyInfo.DeclaringType)) continue;

                    NetSignalInfo notifySignal = null;
                    var notifySignalAttribute = propertyInfo.GetCustomAttribute<NotifySignalAttribute>();
                    if (notifySignalAttribute != null)
                    {
                        var name = notifySignalAttribute.Name;
                        if (string.IsNullOrEmpty(name))
                        {
                            name = $"{propertyInfo.Name}Changed";
                        }
                        
                        if (signals.ContainsKey(name))
                        {
                            notifySignal = signals[name];
                            // Make sure the signal we are referencing has no parameters.
                            if (notifySignal.ParameterCount != 0)
                            {
                                // TODO: They can actually of parameters, but not implemented yet.
                                throw new Exception("Notify signals must have no parameters.");
                            }
                        }
                        else
                        {
                            notifySignal = new NetSignalInfo(type, name);
                            type.AddSignal(notifySignal);
                        }
                    }
                    
                    using (var property = new NetPropertyInfo(
                        type,
                        propertyInfo.Name,
                        NetTypeManager.GetTypeInfo(propertyInfo.PropertyType),
                        propertyInfo.CanRead,
                        propertyInfo.CanWrite,
                        notifySignal))
                    {
                        type.AddProperty(property);
                    }
                }

                // NOTE: This type is going to get a typeInfo object
                // with IsLoading=true and IsLoaded=false. It technically
                // is loaded, but it's misleading.
                InteropBehaviors.OnNetTypeInfoCreated(type, typeInfo);
            }
        }
        
        public void ReleaseNetReference(UInt64 objectId)
        {
            NetReference.OnRelease(objectId);
        }

        public void ReleaseNetDelegateGCHandle(IntPtr handle)
        {
            NetDelegate.ReleaseGCHandle(((GCHandle)handle));
        }

        public IntPtr InstantiateType(IntPtr type)
        {
            try
            {
                var typeName = Interop.NetTypeInfo.GetFullTypeName(type);
                var typeInfo = Type.GetType(typeName);
                if(typeInfo == null) throw new InvalidOperationException($"Invalid type {typeName}");
            
                var netReference = NetReference.CreateForObject(TypeCreator.Create(typeInfo));
                // When .NET collects this NetReference, we don't want it to delete this
                // handle. Ownership has been passed to the caller.
                return Interop.NetReference.Clone(netReference.Handle);
            }
            finally
            {
                Interop.NetTypeInfo.Destroy(type);
            }
        }

        public void ReadProperty(IntPtr p, IntPtr t, IntPtr r)
        {
            using(var property = new NetPropertyInfo(p))
            using(var target = new NetReference(t))
            using(var result = new NetVariant(r))
            {
                var o = target.Instance;

                var propertInfo = o.GetType()
                    .GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);
            
                if(propertInfo == null)
                    throw new InvalidOperationException($"Invalid property {property.Name}");

                Helpers.PackValue(propertInfo.GetValue(o), result);
            }
        }

        public void WriteProperty(IntPtr p, IntPtr t, IntPtr v)
        {
            using (var property = new NetPropertyInfo(p))
            using (var target = new NetReference(t))
            using (var value = new NetVariant(v))
            {
                var o = target.Instance;

                var propertInfo = o.GetType()
                    .GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public);

                if (propertInfo == null)
                    throw new InvalidOperationException($"Invalid property {property.Name}");

                object newValue = null;
                Helpers.Unpackvalue(ref newValue, value);

                propertInfo.SetValue(o, newValue);
            }
        }

        public void InvokeMethod(IntPtr m, IntPtr t, IntPtr p, IntPtr r)
        {
            using (var method = new NetMethodInfo(m))
            using (var target = new NetReference(t))
            using (var parameters = new NetVariantList(p))
            using (var result = r != IntPtr.Zero ? new NetVariant(r) : null)
            {
                var instance = target.Instance;
            
                List<object> methodParameters = null;

                if (parameters.Count > 0)
                {
                    methodParameters = new List<object>();
                    var parameterCount = parameters.Count;
                    for (var x = 0; x < parameterCount; x++)
                    {
                        object v = null;
                        Helpers.Unpackvalue(ref v, parameters.Get(x));
                        methodParameters.Add(v);
                    }
                }

                MethodInfo methodInfo = null;
                var methodName = method.MethodName;
                var methods = instance.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.Name == methodName)
                    .ToList();

                if (methods.Count == 1)
                {
                    methodInfo = methods[0];
                }
                else if (methods.Count > 1)
                {
                    // This is an overload.
                    
                    // TODO: Make this more performant. https://github.com/pauldotknopf/Qml.Net/issues/39
                    
                    // Get all the parameters for the method we are invoking.
                    var parameterTypes = method.GetAllParameters().Select(x => x.Type.FullTypeName).ToList();
                    // And find a good method to invoke.
                    foreach (var potentialMethod in methods)
                    {
                        var potentialMethodParameters = potentialMethod.GetParameters();
                        if (potentialMethodParameters.Length != parameterTypes.Count)
                        {
                            continue;
                        }

                        bool valid = true;
                        for (var x = 0; x < potentialMethodParameters.Length; x++)
                        {
                            if (potentialMethodParameters[x].ParameterType.AssemblyQualifiedName != parameterTypes[x])
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            methodInfo = potentialMethod;
                            break;
                        }
                    }
                }

                if (methodInfo == null)
                {
                    throw new InvalidOperationException($"Invalid method name {method.MethodName}");
                }

                var returnObject = methodInfo.Invoke(instance, methodParameters?.ToArray());
                
                if (result == null)
                {
                    // this method doesn't have return type
                }
                else
                {
                    Helpers.PackValue(returnObject, result);
                }
            }
        }

        public void GCCollect(int maxGeneration)
        {
            GC.Collect(maxGeneration);
        }

        public bool RaiseNetSignals(IntPtr t, string signalName, IntPtr p)
        {
            using (var target = new NetReference(t))
            using (var parameters = p != IntPtr.Zero ? new NetVariantList(p) : null)
            {
                var signals = target.Instance.GetAttachedDelegates(signalName);
                if (signals != null && signals.Count > 0)
                {
                    List<object> methodParameters = null;

                    if (parameters != null && parameters.Count > 0)
                    {
                        methodParameters = new List<object>();
                        var parameterCount = parameters.Count;
                        for (var x = 0; x < parameterCount; x++)
                        {
                            object v = null;
                            Helpers.Unpackvalue(ref v, parameters.Get(x));
                            methodParameters.Add(v);
                        }
                    }

                    foreach (var signal in signals)
                    {
                        signal.DynamicInvoke(methodParameters?.ToArray());
                    }

                    return true; /*some signals were raised*/
                }

                return false; /*no signals were raised*/
            }
        }

        public async Task AwaitTask(IntPtr t, IntPtr c)
        {
            using (var target = new NetReference(t))
            using(var callback = new NetJsValue(c))
            {
                var taskObject = target.Instance;
                if (taskObject is Task task)
                {
                    await task;
                    
                    try
                    {
                        var result = (object)((dynamic)task).Result;
                        callback.Call(result);
                    }
                    catch (RuntimeBinderException)
                    {
                        // No "Result" property, a task with no callbacks.
                        // TODO: Find a better way to handle this than catching an exception.
                        callback.Call();
                    }
                }
                else
                {
                    throw new InvalidOperationException("Attempted to await on a .NET object that wasn't a Task.");
                }
            }
        }
    }
}