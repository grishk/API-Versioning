﻿using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace SelfHost2.Models.Filters {
    public class SwaggerDefaultValues : IOperationFilter {
        /// <summary>
        ///     Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="schemaRegistry">The API schema registry.</param>
        /// <param name="apiDescription">The API description being filtered.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription) {
            operation.deprecated |= apiDescription.IsDeprecated();

            if (operation.parameters == null) {
                return;
            }

            foreach (Parameter parameter in operation.parameters) {
                ApiParameterDescription description = apiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.name);

                if (description == null) {
                    continue;
                }

                // REF: https://github.com/domaindrivendev/Swashbuckle/issues/1101
                if (parameter.description == null) {
                    parameter.description = description.Documentation;
                }

                // REF: https://github.com/domaindrivendev/Swashbuckle/issues/1089
                // REF: https://github.com/domaindrivendev/Swashbuckle/pull/1090
                if (parameter.@default == null) {
                    parameter.@default = description.ParameterDescriptor?.DefaultValue;
                }
            }
        }
    }
}