using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Microsoft.AspNet.OData;
using Swashbuckle.Swagger;

namespace SelfHost2.Models.Filters {
    /// <summary>
    ///     filter for query parameters
    /// </summary>
    public class QueryOperationFiler : IOperationFilter {
        private const string _StringTypeName = "string";
        private const string _ParameterInName = "query";

        private static readonly Dictionary<string, string> _QueryParameters = new Dictionary<string, string> {
            { "$top", "The max number of records" },
            { "$skip", "The number of records to skip" },
            { "$filter", "A function that must evaluate to true for a record to be returned" },
            { "$select", "Specifies a subset of properties to return" },
            { "$orderby", "Determines what values are used to order a collection of records" },
            { "$expand", "Use to add related query data."}
        };

        /// <summary>
        ///     IOperationFilter realization
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription) {
            if (apiDescription.ActionDescriptor.GetCustomAttributes<EnableQueryAttribute>().Any()) {
                operation.parameters = operation.parameters ?? new List<Parameter>();
                SynchronizeOperationParams(operation.parameters);
            } else if(operation.parameters != null) {
                operation.parameters = 
                    operation.parameters
                             .Where(op => _QueryParameters.All(p => op.name != p.Key))
                             .ToList();
            }
        }

        private static void SynchronizeOperationParams(IList<Parameter> parameters) {
            IEnumerable<KeyValuePair<string, string>> pairs = _QueryParameters
                .Where(p => parameters.All(op => op.name != p.Key));

            foreach (KeyValuePair<string, string> pair in pairs) {
                parameters.Add(new Parameter {
                    name = pair.Key,
                    required = false,
                    type = _StringTypeName,
                    @in = _ParameterInName,
                    description = pair.Value
                });
            }
        }
    }
}