using System;
using System.Collections.Generic;

namespace ODataRuntime.Services
{
    public static class ServiceContainer
    {
        private static Dictionary<Type, object> _serviceDictionary = new Dictionary<Type, object>();
        private static object lockObj = new object();

        public static void Add<T>(object o)
        {
            lock (lockObj)
            {
                _serviceDictionary[typeof(T)] = o;
            }
        }
        public static T Get<T>()
        {
            lock (lockObj)
            {
                if (_serviceDictionary.ContainsKey(typeof(T)))
                {
                    return (T)_serviceDictionary[typeof(T)];
                }
            }
            return default;
        }
    }
}
