using ODataRuntime.Models;
using System.Linq;

namespace ODataRuntime.Services
{
    public class EntityServiceKeyInt<TEntity> : BaseEntityService<int, TEntity>
        where TEntity : EntityKeyInt
    {
        protected override int NewKey()
        {
            lock (LockEntities)
            {
                if (Entities.Count == 0) 
                {
                    return 1;
                }
                
                return Entities.Keys.Max() + 1;
            }
        }
    }
}
