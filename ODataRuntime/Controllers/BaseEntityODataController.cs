using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using ODataRuntime.Interfaces;
using ODataRuntime.Models;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;

namespace ODataRuntime.Controllers {
    public abstract class BaseEntityODataController<TKey, TEntity> : ODataController
        where TEntity: BaseEntity<TKey> {
        protected IEntityService<TKey, TEntity> Service { get; }

        public BaseEntityODataController(IEntityService<TKey, TEntity> srv) {
            Service = srv;
        }

        protected async Task<IHttpActionResult> DoDelete(TKey key) {
            bool ret = await Service.Delete(key);
            if (!ret) {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoGet(TKey key) {
            TEntity ret = await Service.Get(key);
            if (ret == null) {
                return NotFound();
            }

            return Ok(ret);
        }

        [EnableQuery]
        [HttpGet]
        [ResponseType(typeof(object[]))]
        public IQueryable<TEntity> Get() => Service.Get();

        protected async Task<IHttpActionResult> DoPatch(TKey key, Delta<TEntity> entity) {
            TEntity ret = await Service.Patch(key, entity);
            if (ret == null) {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoPost(TEntity entity) {
            TEntity ret = await Service.Post(entity);
            if (ret == null) {
                return NotFound();
            }

            return Ok(ret);
        }

        protected async Task<IHttpActionResult> DoPut(TEntity entity) {
            TEntity ret = await Service.Put(entity);
            if (ret == null) {
                return NotFound();
            }

            return Ok(ret);
        }
    }
}