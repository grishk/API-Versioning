using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using ODataRuntime.Interfaces;
using ODataRuntime.Models;
using ODataRuntime.Services;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;

namespace ODataRuntime.Controllers
{
    public abstract class BaseEntityODataController<TKey, TEntity> : ODataController
        where TEntity : BaseEntity<TKey>
    {
        protected IEntityService<TKey, TEntity> Service { get; }
        public BaseEntityODataController()
        {
            Service = ServiceContainer.Get<IEntityService<TKey, TEntity>>();
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
            var ret = await Service.Get(key);
            if (ret == null)
            {
                return NotFound();
            }

            return Ok(ret);
        }

        [EnableQuery]
        [HttpGet]
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
