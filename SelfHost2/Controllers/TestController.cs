using Microsoft.Web.Http;
using System.Web.Http;

namespace SelfHost2.Controllers
{
    [ApiVersion("0.2")]
    [ApiVersion("0.1")]
    public class TestController: ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(new[] {"val1", "val2", "val3"});
        }
    }
}