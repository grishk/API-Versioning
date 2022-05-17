using System.Net;
using System.Net.Http;
using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;

namespace ODataRuntime.Impl.ApiControllers
{
    public class ManagerApi : ModelApi<Manager>
    {
        public override void Register(ControllerBuilder controllerBuilder)
        {
            SetDefaultRoute();
            controllerBuilder.AddVersion("7");
            RegisterGet();
        }
    }
}