/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: https://catlib.io/
 */

using System;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntimeDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace CatLib.ILRuntime.Redirect
{
    /// <summary>
    /// ExtendBindData.cs 重定向
    /// </summary>
    internal static unsafe class RedirectExtendBindData
    {
        /// <summary>
        /// 重定向映射表
        /// </summary>
        private static readonly RedirectMapping mapping;

        /// <summary>
        /// 构建 ExtendBindData.cs 重定向
        /// </summary>
        static RedirectExtendBindData()
        {
            mapping = new RedirectMapping();
            mapping.Register("Alias", 1, 1, Alias_TAlias_IBindData);
            mapping.Register("OnResolving", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`1[T]"
            }, OnResolving_T_IBindData_Action1);
            mapping.Register("OnResolving", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`2[CatLib.IBindData,T]"
            }, OnResolving_T_IBindData_Action2);
            mapping.Register("OnAfterResolving", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`1[T]"
            }, OnAfterResolving_T_IBindData_Action1);
            mapping.Register("OnAfterResolving", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`2[CatLib.IBindData,T]"
            }, OnAfterResolving_T_IBindData_Action2);
            mapping.Register("OnRelease", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`1[T]"
            }, OnRelease_T_IBindData_Action1);
            mapping.Register("OnRelease", 1, 2, new string[]
            {
                "CatLib.IBindData",
                "System.Action`2[CatLib.IBindData,T]"
            }, OnRelease_T_IBindData_Action2);
        }

        /// <summary>
        /// 注册CLR重定向
        /// </summary>
        /// <param name="appDomain">AppDomain</param>
        public static void Register(ILRuntimeDomain appDomain)
        {
            var methods = typeof(ExtendBindData).GetMethods();
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

        // public static IBindData Alias<TAlias>(this IBindData bindData)
        public static StackObject* Alias_TAlias_IBindData(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 1)
            {
                throw new EntryPointNotFoundException();
            }

            var tAlias = Helper.ITypeToService(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 1);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            return ILIntepreter.PushObject(ret, mStack, bindData.Alias(tAlias));
        }

        // public static IBindData OnResolving<T>(this IBindData bindData, Action<T> closure)
        public static StackObject* OnResolving_T_IBindData_Action1(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnResolving((_, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }

        // public static IBindData OnResolving<T>(this IBindData bindData, Action<IBindData, T> closure)
        public static StackObject* OnResolving_T_IBindData_Action2(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnResolving((bind, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(bind, instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }

        // public static IBindData OnAfterResolving<T>(this IBindData bindData, Action<T> closure)
        public static StackObject* OnAfterResolving_T_IBindData_Action1(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnAfterResolving((_, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }

        // public static IBindData OnAfterResolving<T>(this IBindData bindData, Action<IBindData, T> closure)
        public static StackObject* OnAfterResolving_T_IBindData_Action2(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnAfterResolving((bind, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(bind, instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }

        // public static IBindData OnRelease<T>(this IBindData bindData, Action<T> closure)
        public static StackObject* OnRelease_T_IBindData_Action1(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnRelease((_, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }

        // public static IBindData OnRelease<T>(this IBindData bindData, Action<IBindData, T> closure)
        public static StackObject* OnRelease_T_IBindData_Action2(ILIntepreter intp, StackObject* esp, IList<object> mStack,
            CLRMethod method, bool isNewObj)
        {
            var genericArguments = method.GenericArguments;
            if (genericArguments == null || genericArguments.Length != 1 || method.ParameterCount != 2)
            {
                throw new EntryPointNotFoundException();
            }

            var tWhere = Helper.ITypeToClrType(genericArguments[0]);
            var ret = ILIntepreter.Minus(esp, 2);

            var ptrOfThisMethod = ILIntepreter.Minus(esp, 1);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var closure =
                (Delegate)typeof(Delegate).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            ptrOfThisMethod = ILIntepreter.Minus(esp, 2);
            ptrOfThisMethod = ILIntepreter.GetObjectAndResolveReference(ptrOfThisMethod);

            var bindData =
                (IBindData)typeof(IBindData).CheckCLRTypes(
                    StackObject.ToObject(ptrOfThisMethod, intp.AppDomain, mStack));

            intp.Free(ptrOfThisMethod);

            bindData.OnRelease((bind, instance) =>
            {
                if (tWhere.IsInstanceOfType(instance))
                {
                    closure.DynamicInvoke(bind, instance);
                }
            });

            return ILIntepreter.PushObject(ret, mStack, bindData);
        }
    }
}
