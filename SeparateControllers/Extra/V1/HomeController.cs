﻿using System.Web.Http;
using Microsoft.Web.Http;

namespace SeparateControllers.Extra.V1
{
    [ApiVersion("1.0")]
    [ControllerName("home")]
    //[Route("api/v{version:apiVersion}/home/{?id}")]
    public class HomeController : ApiController
    {
        public HomeController()
        {
        }

        public string Get(int id)
        {
            return "HomeV1";
        }
    }

  
}
