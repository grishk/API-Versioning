using Asp.Versioning;

namespace Microsoft.Examples
{
	using Microsoft.AspNet.OData;
	using Microsoft.AspNet.OData.Routing;
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

        [HttpGet]
        [ResponseType(typeof(double))]
        [ODataRoute("Healther(Family={family})")]
        public IHttpActionResult Healther(string family) => Ok(99.9);

    }
}