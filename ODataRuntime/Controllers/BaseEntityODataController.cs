using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using ODataRuntime.Interfaces;
using ODataRuntime.Models;
using ODataRuntime.Services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ODataRuntime.Controllers
{
    public abstract class BaseEntityODataController<TKey, TEntity> : ODataController
        where TEntity : BaseEntity<TKey>
    {
        protected IEntityService<TKey, TEntity> Service { get; }
        public BaseEntityODataController(IEntityService<TKey, TEntity> srv)
        {
            Service = srv;
        }

        protected async Task<IHttpActionResult> DoDelete(TKey key) 
        {
            var ret = await Service.Delete(key);
            if (!ret) 
            {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoGet(TKey key)
        {
            var svc = this.RequestContext.Configuration.DependencyResolver.GetService(typeof(IEntityService<TKey, TEntity>));

            var ret = await Service.Get(key);
            if (ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }

        [EnableQuery]
        [HttpGet]
        [ResponseType(typeof(object[]))]
        public async Task<IHttpActionResult> Get(ODataQueryOptions<TEntity> options)
        {
            var ret = await Service.Get(options);
            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoPatch(TKey key, Delta<TEntity> entity)
        {
            var ret = await Service.Patch(key, entity);
            if (ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoPost(TEntity entity)
        {
            var ret = await Service.Post(entity);
            if (ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoPut(TEntity entity)
        {
            var ret = await Service.Put(entity);
            if (ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }
    }
}
