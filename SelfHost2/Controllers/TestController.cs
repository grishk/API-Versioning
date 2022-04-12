using System.Web.Http;

namespace SelfHost2.Controllers
{
    
    public class TestController: ApiController
    {
        [HttpGet]
        [Route("api/tests")]
        public IHttpActionResult Get()
        {
            return Ok(new[] {"val1", "val2", "val3"});
        }
    }
}