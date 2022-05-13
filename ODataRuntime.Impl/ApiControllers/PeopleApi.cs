using ODataRuntime.Builders;
using ODataRuntime.Impl.Models;
using ODataRuntime.Interfaces;

namespace ODataRuntime.Impl.ApiControllers
{
    public class PeopleApi: Api<Person>
    {
        public override void Register(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.AddODataRoutePrefix(nameof(Person));
            controllerBuilder.AddVersion("0.3");
            var actionBuilderGet = new ActionBuilderFromBaseMethod(controllerBuilder, "Get", "DoGet");
        }
    }
}