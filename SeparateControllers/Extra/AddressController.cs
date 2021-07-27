using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;


namespace SeparateControllers.Extra
{
    [ODataRoutePrefix("addresses")]
    public class AddressController : ODataController
    {
        private readonly IDbContext _dbContext;

        public AddressController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [EnableQuery(PageSize = 50)]
        public IQueryable<Address> Get()
        {
            return _dbContext.Addresses;
        }

        [ODataRoute("{id}")]
        public SingleResult<Address> Get(int id)
        {
            return SingleResult.Create(_dbContext.Addresses.Where(x => x.Id == id));
        }

        [HttpDelete]
        [ODataRoute("{id}")]
        public IHttpActionResult Delete(int id)
        {
            _dbContext.Delete(_dbContext.Addresses.Single(x => x.Id == id));
            return Ok();
        } 
        
        [HttpPost]
        public IHttpActionResult Post([FromBody]Address model)
        {
           return Created(_dbContext.Create(model));
        }

        [HttpPut]
        [ODataRoute("{id}")]
        public IHttpActionResult Put(int id, [FromBody] Address model)
        {
            return Ok(_dbContext.Update(model));
        }
    }

    public interface IDbContext
    {
        IQueryable<Address> Addresses { get; }
        void Delete(object entity);
        object Create(object entity);
        object Update(object model);
    }

    public class Address
    {
        public int Id { get; set; }
    }
}