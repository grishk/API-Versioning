using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.Description;

namespace SeparateControllers.DynamicBuilder.Extensions
{
    public static class MethodBuilderController
    {
        private readonly static ConstructorInfo HttpGetConstructor = typeof(HttpGetAttribute).GetConstructor(Type.EmptyTypes);
        private readonly static ConstructorInfo HttpDeleteConstructor = typeof(HttpDeleteAttribute).GetConstructor(Type.EmptyTypes);
        private readonly static ConstructorInfo HttpPatchConstructor = typeof(HttpPatchAttribute).GetConstructor(Type.EmptyTypes);
        private readonly static ConstructorInfo HttpPutConstructor = typeof(HttpPutAttribute).GetConstructor(Type.EmptyTypes);
        private readonly static ConstructorInfo HttpPostConstructor = typeof(HttpPostAttribute).GetConstructor(Type.EmptyTypes);
        private readonly static ConstructorInfo ODataRouteConstructor = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        private readonly static ConstructorInfo SwaggerResponseConstructor = typeof(SwaggerResponseAttribute).GetConstructor(new[] { typeof(HttpStatusCode), typeof(string), typeof(Type) });
        private readonly static ConstructorInfo ResponseTypeConstructor = typeof(ResponseTypeAttribute).GetConstructor(new[] { typeof(Type) });

        private readonly static ConstructorInfo FromODataUriConstructor = typeof(FromODataUriAttribute).GetConstructor(Type.EmptyTypes);

        private static List<Delegate> _delegates = new List<Delegate>();

        public static object DoDelegateRet(object[] parameters, int index) 
        {
            var ret = _delegates[index].DynamicInvoke(parameters);
            return ret;
        }

        private static object delegateLock = new object();
        private static int AddDelegate(Delegate dlgt)
        {
            lock (delegateLock) 
            {
                _delegates.Add(dlgt);
                return _delegates.Count-1;
            }
        }

        private static MethodInfo _doDelegateRetMethodInfo;
        private static MethodInfo GetDoDelegateRetPropertyInfo() 
        {
            if (_doDelegateRetMethodInfo == null) 
            {
                _doDelegateRetMethodInfo = typeof(MethodBuilderController)
                    .GetMethod(nameof(MethodBuilderController.DoDelegateRet), BindingFlags.Public | BindingFlags.Static);
            }

            return _doDelegateRetMethodInfo;
        }

        public static MethodBuilder AddHttpVerb(this MethodBuilder methodBuilder, HttpMethod method)
        {
            if (method == HttpMethod.Get)
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpGetConstructor, new object[0]));
            else if (method == HttpMethod.Post)
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpPostConstructor, new object[0]));
            else if (method == HttpMethod.Put)
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpPutConstructor, new object[0]));
            else if (method == HttpMethod.Delete)
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(HttpDeleteConstructor, new object[0]));

            return methodBuilder;
        }

        public static MethodBuilder AddODataRoute(this MethodBuilder methodBuilder, string route)
        {
            methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(ODataRouteConstructor, new object[] { route }));

            return methodBuilder;
        }

        public static MethodBuilder AddSwaggerResponse(this MethodBuilder methodBuilder, 
            HttpStatusCode status, string description = null, Type returnType = null)
        {
            methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(SwaggerResponseConstructor, 
                new object[] { status, description, returnType }));

            return methodBuilder;
        }

        public static MethodBuilder AddResponseType(this MethodBuilder methodBuilder, Type type)
        {
            methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(ResponseTypeConstructor, new object[] { type }));

            return methodBuilder;
        }

        public static MethodBuilder SetDelegate(this MethodBuilder methodBuilder, Delegate method)
        {
            var prms = method.Method.GetParameters();

            var generator = methodBuilder.GetILGenerator();

            var index = AddDelegate(method);


            generator.Emit(OpCodes.Ldc_I4, prms.Length);
            generator.Emit(OpCodes.Newarr, typeof(object));

            for (var i = 0; i < prms.Length; i++)
            {
                var prm = prms[i];
                methodBuilder.DefineParameter(i + 1, ParameterAttributes.In, prm.Name);

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

            if (methodBuilder.ReturnType.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, methodBuilder.ReturnType);
            }

            generator.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        public static MethodBuilder AddParameter(this MethodBuilder methodBuilder, int order, string name)
        {
            methodBuilder.DefineParameter(order, ParameterAttributes.In, name);

            return methodBuilder;
        }
    }
}
