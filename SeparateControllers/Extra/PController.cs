namespace Microsoft.Examples
{
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.Web.Http;
    using System.Web.Http;
    using System.Web.Http.Description;

    /// <summary>
    /// Provides unbound, utility functions.
    /// </summary>
    [ApiVersionNeutral]
    public abstract class PController : ODataController
    {
        [HttpGet]
        [ResponseType(typeof(int))]
        [ODataRoute("Pinging(IP={ip})")]
        public IHttpActionResult Pinging(int ip) => Ok(15);
    }
}