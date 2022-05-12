using ODataRuntime.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class ActionBuilderFromDelegate : ActionBuilder
    {
        private readonly Delegate _Method;
        private PropertyInfo _serviceProperty { get; }

        public ActionBuilderFromDelegate(ControllerBuilder controllerBuilder, string actioName, Delegate method)
        {
            _serviceProperty = controllerBuilder.ServiceProperty;
            _Method = method;
            var returnType = _Method.Method.ReturnType;
            var parameters = _Method.Method.GetParameters()
                .Where(p => !IsServiceParameter(p))
                .Select(p=>p.ParameterType)
                .ToArray();

            _methodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
            SetDelegate();
        }

        private ActionBuilder SetDelegate()
        {
            ParameterInfo[] prms = _Method.Method.GetParameters();
            ILGenerator generator = _methodBuilder.GetILGenerator();
            int index = AddDelegate(_Method);

            generator.Emit(OpCodes.Ldc_I4, prms.Length);
            generator.Emit(OpCodes.Newarr, typeof(object));

            bool hasService = false;
            for (var i = 0; i < prms.Length; i++)
            {
                var prm = prms[i];

                generator.Emit(OpCodes.Dup);
                generator.Emit(OpCodes.Ldc_I4, i);

                if (IsServiceParameter(prm)) {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Callvirt, _serviceProperty.GetMethod);
                    hasService = true;
                } else {
                    int pos = (hasService ? i : i + 1);
                    generator.Emit(OpCodes.Ldarg, pos);
                    _methodBuilder.DefineParameter(pos, ParameterAttributes.In, prm.Name);
                }

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

        private bool IsServiceParameter(ParameterInfo parameter) {
            if (_serviceProperty == null) {
                return false;
            }

            Type pType = parameter.ParameterType;

            if (pType.IsInterface
                && pType.IsGenericType
                && pType.GetGenericTypeDefinition() == typeof(IEntityService<,>)) {
                return true;
            }

            var ret = pType.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityService<,>));

            return ret;
        }

        public static object DoDelegateRet(object[] parameters, int index)
        {
            var ret = _Delegates[index].DynamicInvoke(parameters);
            return ret;
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
