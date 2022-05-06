using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class ActionBuilderFromDelegate : ActionBuilder
    {
        private static List<Delegate> _delegates = new List<Delegate>();
        private static object delegateLock = new object();

        private readonly Delegate _method;

        public ActionBuilderFromDelegate(ControllerBuilder controllerBuilder, string actioName, Delegate method)
        {
            _method = method;
            var returnType = _method.Method.ReturnType;
            var parameters = _method.Method.GetParameters()
                .Select(p=>p.ParameterType)
                .ToArray();

            _methodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
            SetDelegate();
        }

        private ActionBuilder SetDelegate()
        {
            var prms = _method.Method.GetParameters();

            var generator = _methodBuilder.GetILGenerator();

            var index = AddDelegate(_method);


            generator.Emit(OpCodes.Ldc_I4, prms.Length);
            generator.Emit(OpCodes.Newarr, typeof(object));

            for (var i = 0; i < prms.Length; i++)
            {
                var prm = prms[i];
                _methodBuilder.DefineParameter(i + 1, ParameterAttributes.In, prm.Name);

                generator.Emit(OpCodes.Dup);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Ldarg, i + 1);

                if (prm.ParameterType.IsValueType)
                {
                    generator.Emit(OpCodes.Box, prm.ParameterType);
                }

                generator.Emit(OpCodes.Stelem_Ref);
            }

            generator.Emit(OpCodes.Ldc_I4, index);

            generator.Emit(OpCodes.Call, GetDoDelegateRetPropertyInfo());

            if (_methodBuilder.ReturnType.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, _methodBuilder.ReturnType);
            }

            generator.Emit(OpCodes.Ret);

            return this;
        }

        public static object DoDelegateRet(object[] parameters, int index)
        {
            var ret = _delegates[index].DynamicInvoke(parameters);
            return ret;
        }

        private static int AddDelegate(Delegate dlgt)
        {
            lock (delegateLock)
            {
                _delegates.Add(dlgt);
                return _delegates.Count - 1;
            }
        }

        private static MethodInfo _doDelegateRetMethodInfo;
        private static MethodInfo GetDoDelegateRetPropertyInfo()
        {
            if (_doDelegateRetMethodInfo == null)
            {
                _doDelegateRetMethodInfo = typeof(ActionBuilderFromDelegate)
                    .GetMethod(nameof(DoDelegateRet), BindingFlags.Public | BindingFlags.Static);
            }

            return _doDelegateRetMethodInfo;
        }

    }
}
