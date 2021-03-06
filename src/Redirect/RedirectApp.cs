﻿/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: https://catlib.io/
 */

using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections.Generic;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntimeDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace CatLib.ILRuntime.Redirect
{
    /// <summary>
    /// App.cs 重定向
    /// </summary>
    internal static unsafe partial class RedirectApp
    {
        /// <summary>
        /// 重定向映射表
        /// </summary>
        private static readonly RedirectMapping mapping;

        /// <summary>
        /// 构建 App.cs 重定向
        /// </summary>
        static RedirectApp()
        {
            mapping = new RedirectMapping();

            mapping.Register("GetBind", 1, 0, GetBind_TService);
            mapping.Register("HasInstance", 1, 0, HasInstance_TService);
            mapping.Register("IsResolved", 1, 0, IsResolved_TService);
            mapping.Register("HasBind", 1, 0, HasBind_TService);
            mapping.Register("CanMake", 1, 0, CanMake_TService);
            mapping.Register("IsStatic", 1, 0, IsStatic_TService);
            mapping.Register("IsAlias", 1, 0, IsAlias_TService);
            mapping.Register("Alias", 2, 0, Alias_TAlias_TService);
            mapping.Register("Unbind", 1, 0, Unbind_TService);
            mapping.Register("Tag", 1, 1, Tag_TService_String);
            mapping.Register("Instance", 1, 1, Instance_TService_Object);
            mapping.Register("Release", 1, 0, Release_TService);
            mapping.Register("Make", 1, 1, Make_TService_ArrObject);
            mapping.Register("Factory", 1, 1, Factory_TService_ArrObject);
            mapping.Register("Type2Service", 1, 0, Type2Service_TService);

            RegisterExtend();
            RegisterBind();
            RegisterBindIf();
            RegisterSingleton();
            RegisterSingletonIf();
            RegisterOnRelease();
            RegisterOnResolving();
            RegisterOnAfterResolving();
            RegisterWatch();
        }

        /// <summary>
        /// 注册CLR重定向
        /// </summary>
        /// <param name="appDomain">AppDomain</param>
        public static void Register(ILRuntimeDomain appDomain)
        {
            var methods = typeof(App).GetMethods();
            foreach (var method in methods)
            {
                var redirection = mapping.GetRedirection(method);
                if (redirection == null)
                {
                    continue;
                }

                appDomain.RegisterCLRMethodRedirection(method, redirection);
            }
        }

        // public static IBindData GetBind<TService>()
        private static StackObject* GetBind_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.GetBind(tService));
        }

        // public static bool HasInstance<TService>()
        private static StackObject* HasInstance_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.HasInstance(tService));
        }

        // public static bool IsResolved<TService>()
        private static StackObject* IsResolved_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.IsResolved(tService));
        }

        // public static bool HasBind<TService>()
        private static StackObject* HasBind_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.HasBind(tService));
        }

        // public static bool CanMake<TService>()
        private static StackObject* CanMake_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.CanMake(tService));
        }

        // public static bool IsStatic<TService>()
        private static StackObject* IsStatic_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.IsStatic(tService));
        }

        // public static bool IsAlias<TService>()
        private static StackObject* IsAlias_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.IsAlias(tService));
        }

        // public static IContainer Alias<TAlias, TService>()
        private static StackObject* Alias_TAlias_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 2 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tAlias = Helper.ITypeToService(genericArguments[0]);
            var tService = Helper.ITypeToService(genericArguments[1]);

            return ILIntepreter.PushObject(esp, mStack, App.Alias(tAlias, tService));
        }

        // public static void Unbind<TService>()
        private static StackObject* Unbind_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);
            App.Unbind(tService);

            return esp;
        }

        // public static void Tag<TService>(string tag)
        private static StackObject* Tag_TService_String(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 1)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var tag =
                (string)typeof(string).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            App.Tag(tag, tService);

            return ILIntepreter.Minus(esp, 1);
        }

        // public static void Instance<TService>(object instance)
        private static StackObject* Instance_TService_Object(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 1)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var instance =
                typeof(object).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            App.Instance(tService, instance);

            return ILIntepreter.Minus(esp, 1);
        }

        // public static bool Release<TService>()
        private static StackObject* Release_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, App.Release(tService));
        }

        // public static TService Make<TService>(params object[] userParams)
        private static StackObject* Make_TService_ArrObject(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 1)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var userParams =
                (object[])typeof(object[]).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            return ILIntepreter.PushObject(ILIntepreter.Minus(esp, 1), mStack, App.Make(tService, userParams));
        }

        // public static Func<TService> Factory<TService>(params object[] userParams)
        private static StackObject* Factory_TService_ArrObject(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 1)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var userParams =
                (object[])typeof(object[]).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            object result;
            if (genericArguments[0] is ILType)
            {
                Func<ILTypeInstance> func = () => (ILTypeInstance) App.Make(tService, userParams);
                result = func;
            }
            else
            {
                throw new NotSupportedException("Only use the factory to hotfix services.");
            }

            return ILIntepreter.PushObject(ILIntepreter.Minus(esp, 1), mStack, result);
        }

        // public static bool Type2Service<TService>()
        private static StackObject* Type2Service_TService(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 0)
            {
                throw new EntryPointNotFoundException();
            }

            var tService = Helper.ITypeToService(genericArguments[0]);

            return ILIntepreter.PushObject(esp, mStack, tService);
        }
    }
}
