using System.Collections.Generic;
using Microsoft.AspNet.OData.Routing;
using SeparateControllers.HttpExtensions;
using SeparateControllers.Models;
using Swashbuckle.Swagger;

namespace SelfHost2.Controllers
{
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.Web.Http;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;

    /// <summary>
    /// Represents a RESTful people service.
    /// </summary>
    [ApiVersion("0.2")]
    [ApiVersion("0.1", Deprecated = true)]
    public class PeopleController : ODataController
    {
        /// <summary>
        /// Gets a single person.
        /// </summary>
        /// <param name="key">The requested person identifier.</param>
        /// <param name="options">The current OData query options.</param>
        /// <returns>The requested person.</returns>
        /// <response code="200">The person was successfully retrieved.</response>
        /// <response code="404">The person does not exist.</response>
        [HttpGet]
        //[ResponseType(typeof(Person))]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(200, "Get Person", typeof(Person))]
        [Swashbuckle.Swagger.Annotations.SwaggerOperation("PersonOperation")]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(300)]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(301)]
        
        public IHttpActionResult Get(int key, ODataQueryOptions<Person> options)
        {
            var people = new[]
            {
                new Person()
                {
                    Id = key,
                    FirstName = "John",
                    LastName = "Doe",
                }
            };

            return this.SuccessOrNotFound(options.ApplyTo(people.AsQueryable()).SingleOrDefault());
        }

       [HttpGet]
       [Swashbuckle.Swagger.Annotations.SwaggerResponse(200, "Get People", typeof(Order))]
       [Swashbuckle.Swagger.Annotations.SwaggerOperation("PeopleOperation")]
       [Swashbuckle.Swagger.Annotations.SwaggerResponse(300)]
       [Swashbuckle.Swagger.Annotations.SwaggerResponse(301)]
        public IHttpActionResult Get(ODataQueryOptions<Order> options)
        {
            return this.SuccessOrNotFound(new Order{Id = 1, Customer = "Cust1"});
        }
    }
}