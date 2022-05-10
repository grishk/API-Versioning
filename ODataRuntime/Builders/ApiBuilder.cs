using ODataRuntime.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ODataRuntime.Builders
{
    public class ApiBuilder
    {
        private const string defaultAssemblyName = "DynamycApi";

        private static int counter;
        private List<Api> _container = new List<Api>();
        protected readonly AssemblyBuilder _assemblyBuilder;

        public ApiBuilder(string assemblyName = null)
        {
            
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                int next = Interlocked.Increment(ref counter);
                assemblyName = $"{defaultAssemblyName}{next}";
            }

            _assemblyBuilder = new AssemblyBuilder(assemblyName);
        }

        public ApiBuilder AddApi(Api api) 
        {
            _container.Add(api);

            return this;
        }

        public ApiBuilder Build() 
        {
            _container.ForEach(api => api.Create(_assemblyBuilder));
            _container.Clear();

            return this;
        }

        public ApiBuilder CofigureApiBuilder(Action<ApiBuilder> configure)
        {
            if (configure == null) 
            {
                throw new ArgumentNullException();
            }

            configure(this);

            return this;
        }
    }
}
