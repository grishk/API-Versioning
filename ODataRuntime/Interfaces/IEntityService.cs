using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using ODataRuntime.Models;

namespace ODataRuntime.Interfaces {
    public interface IEntityService<TKey, TEntity>
        where TEntity: BaseEntity<TKey> {
        Task<TEntity> Get(TKey key);
        IQueryable<TEntity> Get();
        Task<TEntity> Post(TEntity entity);
        Task<TEntity> Put(TEntity entity);
        Task<TEntity> Patch(TKey key, Delta<TEntity> data);
        Task<bool> Delete(TKey key);
    }
}