using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ODataRuntime.Builders.Helpers {
    public static class TypeBuilderHelper {
        /// <summary>
        ///     Creates one constructor for each public constructor in the base class. Each constructor simply
        ///     forwards its arguments to the base constructor, and matches the base constructor's signature.
        ///     Supports optional values, and custom attributes on constructors and parameters.
        ///     Does not support n-ary (variadic) constructors
        /// </summary>
        public static void CreatePassThroughConstructors(this TypeBuilder builder, Type baseType) {
            foreach (ConstructorInfo constructor in baseType.GetConstructors()) {
                ParameterInfo[] parameters = constructor.GetParameters();

                if (IsToBeSkipped(parameters)) {
                    continue;
                }

                ConstructorBuilder ctor = builder.CreateCtorBuilder(parameters, constructor);
                ctor.GenerateILCode(parameters, constructor);
            }
        }

        private static void GenerateILCode(this ConstructorBuilder ctor, ParameterInfo[] parameters, ConstructorInfo constructor) {
            ILGenerator emitter = ctor.GetILGenerator();
            emitter.Emit(OpCodes.Nop);
            // Load `this` and call base constructor with arguments
            emitter.Emit(OpCodes.Ldarg_0);

            for (var i = 1; i <= parameters.Length; ++i) {
                emitter.Emit(OpCodes.Ldarg, i);
            }

            emitter.Emit(OpCodes.Call, constructor);
            emitter.Emit(OpCodes.Ret);
        }

        private static bool IsToBeSkipped(ParameterInfo[] parameters) {
            return parameters.Length > 0 && parameters.Last().IsDefined(typeof(ParamArrayAttribute), false);
        }

        private static ConstructorBuilder CreateCtorBuilder(this TypeBuilder builder, ParameterInfo[] parameters, ConstructorInfo constructor) {
            Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
            Type[][] requiredCustomModifiers = parameters.Select(p => p.GetRequiredCustomModifiers()).ToArray();
            Type[][] optionalCustomModifiers = parameters.Select(p => p.GetOptionalCustomModifiers()).ToArray();
            ConstructorBuilder ctor = builder.DefineConstructor(MethodAttributes.Public, constructor.CallingConvention,
                                                                parameterTypes, requiredCustomModifiers,
                                                                optionalCustomModifiers);
            ctor.SetCtorParameters(parameters);
            ctor.SetCtorAttributes(constructor);

            return ctor;
        }

        private static void SetCtorAttributes(this ConstructorBuilder ctor, ConstructorInfo constructor) {
            foreach (CustomAttributeBuilder attribute in BuildCustomAttributes(constructor.GetCustomAttributesData())) {
                ctor.SetCustomAttribute(attribute);
            }
        }

        private static void SetCtorParameters(this ConstructorBuilder ctor, ParameterInfo[] parameters) {
            for (var i = 0; i < parameters.Length; ++i) {
                ParameterInfo parameter = parameters[i];
                ParameterBuilder parameterBuilder = ctor.DefineParameter(i + 1, parameter.Attributes, parameter.Name);

                if (((int)parameter.Attributes & (int)ParameterAttributes.HasDefault) != 0) {
                    parameterBuilder.SetConstant(parameter.RawDefaultValue);
                }

                parameterBuilder.SetParameterAttributes(parameter);
            }
        }

        private static void SetParameterAttributes(this ParameterBuilder parameterBuilder, ParameterInfo parameter) {
            foreach (CustomAttributeBuilder attribute in BuildCustomAttributes(parameter.GetCustomAttributesData())) {
                parameterBuilder.SetCustomAttribute(attribute);
            }
        }

        private static CustomAttributeBuilder[] BuildCustomAttributes(IEnumerable<CustomAttributeData> customAttributes) {
            return customAttributes.Select(attribute => {
                object[] attributeArgs = attribute.ConstructorArguments.Select(a => a.Value).ToArray();
                PropertyInfo[] namedPropertyInfos = attribute.NamedArguments.Select(a => a.MemberInfo).OfType<PropertyInfo>().ToArray();
                object[] namedPropertyValues = attribute.NamedArguments.Where(a => a.MemberInfo is PropertyInfo).Select(a => a.TypedValue.Value).ToArray();
                FieldInfo[] namedFieldInfos = attribute.NamedArguments.Select(a => a.MemberInfo).OfType<FieldInfo>().ToArray();
                object[] namedFieldValues = attribute.NamedArguments.Where(a => a.MemberInfo is FieldInfo).Select(a => a.TypedValue.Value).ToArray();
                return new CustomAttributeBuilder(attribute.Constructor, attributeArgs, namedPropertyInfos, namedPropertyValues, namedFieldInfos, namedFieldValues);
            }).ToArray();
        }
    }
}