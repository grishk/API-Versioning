using ODataRuntime.Models;
using System.Linq;

namespace ODataRuntime.Services
{
    public class EntityServiceKeyInt<TEntity> : BaseEntityService<int, TEntity>
        where TEntity : EntityKeyInt
    {
        protected override int NewKey()
        {
            lock (lockEntitis)
            {
                if (entitis.Count == 0) 
                {
                    return 1;
                }
                
                return entitis.Keys.Max() + 1;
            }
        }
    }
}
