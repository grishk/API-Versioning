using ODataRuntime.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ODataRuntime.Builders
{
    public class ApiBuilder
    {
        private const string _DefaultAssemblyName = "DynamycApi";

        private static int _Counter;
        private readonly List<Api> _Container = new List<Api>();
        private readonly AssemblyBuilder _AssemblyBuilder;

        public ApiBuilder(string assemblyName = null)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                int next = Interlocked.Increment(ref _Counter);
                assemblyName = $"{_DefaultAssemblyName}{next}";
            }

            _AssemblyBuilder = new AssemblyBuilder(assemblyName);
        }

        public ApiBuilder AddApi(Api api) 
        {
            _Container.Add(api);

            return this;
        }

        public ApiBuilder Build() 
        {
            _Container.ForEach(api => api.Create(_AssemblyBuilder));
            _Container.Clear();

            return this;
        }

        public ApiBuilder ConfigureApiBuilder(Action<ApiBuilder> configure)
        {
            if (configure == null) 
            {
                throw new ArgumentNullException(nameof(configure));
            }

            configure(this);

            return this;
        }
    }
}
