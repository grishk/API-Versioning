﻿using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders
{
    public class ActionBuilderFromBaseMethod: ActionBuilder
    {
        private readonly MethodInfo _baseMethodInfo;

        public ActionBuilderFromBaseMethod(ControllerBuilder controllerBuilder, string actioName, string methodName)
        {
            _baseMethodInfo = controllerBuilder.BaseControllerType
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance); ;
            var returnType = _baseMethodInfo.ReturnType;
            var parameters = _baseMethodInfo.GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

            _methodBuilder = controllerBuilder.CreateActionBuilder(actioName, returnType, parameters);
            SetBaseMethod();
        }

        public ActionBuilder SetBaseMethod()
        {
            var prms = _baseMethodInfo.GetParameters();

            var generator = _methodBuilder.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < prms.Length; i++)
            {
                var prm = prms[i];
                var paramInfo = _methodBuilder.DefineParameter(i + 1, ParameterAttributes.In, prm.Name);

                if (prm.Name.Equals("key") || prm.Name.Equals("id"))
                {
                    paramInfo.SetCustomAttribute(new CustomAttributeBuilder(FromODataUriConstructor, new object[0]));
                }

                generator.Emit(OpCodes.Ldarg, i + 1);
            }


            generator.Emit(OpCodes.Call, _baseMethodInfo);

            generator.Emit(OpCodes.Ret);

            return this;
        }
    }
}