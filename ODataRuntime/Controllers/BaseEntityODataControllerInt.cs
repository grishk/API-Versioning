
using ODataRuntime.Interfaces;
using ODataRuntime.Models;

namespace ODataRuntime.Controllers
{
    public class BaseEntityODataControllerInt<TEntity> : BaseEntityODataController<int, TEntity>
        where TEntity : BaseEntity<int>
    {
        public BaseEntityODataControllerInt(IEntityService<int, TEntity> srv) : base(srv)
        {
        }
    }
}
