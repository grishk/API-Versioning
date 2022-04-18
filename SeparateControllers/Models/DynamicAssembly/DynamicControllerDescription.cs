using System;
using System.Web.Http.Controllers;

namespace SeparateControllers.Models.DynamicAssembly
{
    public class DynamicControllerDescription
    {
        public static DynamicControllerDescription Create<T>(string name) 
            where T: IHttpController
        {
            var ret = new DynamicControllerDescription
            {
                Name = name,
                ParentType = typeof(T)
            };

            return ret;
        }

        public string Name { get; private set; }
        public Type ParentType { get; private set; }
        public string[] Versions { get; set; }
        public string ODataPrefix { get; set; }
    }
}
