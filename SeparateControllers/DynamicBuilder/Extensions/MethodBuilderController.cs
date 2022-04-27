using Microsoft.AspNet.OData.Routing;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;
using System;
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
        private readonly static ConstructorInfo HttpGetConstructor = typeof(HttpGetAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo HttpDeleteConstructor = typeof(HttpDeleteAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo HttpPatchConstructor = typeof(HttpPatchAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo HttpPutConstructor = typeof(HttpPutAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo HttpPostConstructor = typeof(HttpPostAttribute).GetConstructor(new Type[0]);
        private readonly static ConstructorInfo ODataRouteConstructor = typeof(ODataRouteAttribute).GetConstructor(new[] { typeof(string) });
        private readonly static ConstructorInfo SwaggerResponseConstructor = typeof(SwaggerResponseAttribute).GetConstructor(new[] { typeof(HttpStatusCode), typeof(string), typeof(Type) });
        private readonly static ConstructorInfo ResponseTypeConstructor = typeof(ResponseTypeAttribute).GetConstructor(new[] { typeof(Type) });

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
            methodBuilder.DefineParameter(1, ParameterAttributes.In, prms[0].Name);

            // get a MethodInfo pointer to the Math.Pow(double, double) static
            // method that we are willing to use in our dynamic method
            var abs = typeof(Math).GetMethod("Abs", new[] { typeof(int) });

            var il = methodBuilder.GetILGenerator();
            // Push the first argument onto the evaluation stack
            il.Emit(OpCodes.Ldarg_0);
            // Invoke the Math.Pow static method that we obtained a MethodInfo earlier
            // by passing the two arguments that are on the evaluation stack
            il.Emit(OpCodes.Call, abs);

            // Return from the method pushing a return value from the callee's evaluation stack onto the caller's evaluation stack
            il.Emit(OpCodes.Ret);

            return methodBuilder;


            var generator = methodBuilder.GetILGenerator();
            // Push the first argument onto the evaluation stack
            generator.Emit(OpCodes.Ldarg_0);
            // Invoke the Math.Pow static method that we obtained a MethodInfo earlier
            // by passing the two arguments that are on the evaluation stack
            generator.Emit(OpCodes.Callvirt, method.Method);

            // Return from the method pushing a return value from the callee's evaluation stack onto the caller's evaluation stack
            generator.Emit(OpCodes.Ret);

            return methodBuilder;


            for (var i = 0; i < prms.Length; i++)
            {
                var prm = prms[i];
                generator.Emit(OpCodes.Ldarg_S, i);
                methodBuilder.DefineParameter(i + 1, ParameterAttributes.In, prm.Name);
            }
            generator.Emit(OpCodes.Call, method.Method);
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
