using ODataRuntime.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODataRuntime.Interfaces
{
    public interface IEntityService<TKey, TEntity> 
        where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> Get(TKey key);
        Task<IEnumerable<TEntity>> Get(ODataQueryOptions<TEntity> options);
        Task<TEntity> Post(TEntity entity);
        Task<TEntity> Put(TEntity entity);
        Task<TEntity> Patch(TKey key, Delta<TEntity> data);
        Task<bool> Delete(TKey key);
    }
}
