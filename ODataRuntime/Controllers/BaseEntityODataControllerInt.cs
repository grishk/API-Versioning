
using ODataRuntime.Models;

namespace ODataRuntime.Controllers
{
    public class BaseEntityODataControllerInt<TEntity>: BaseEntityODataController<int,TEntity>
        where TEntity : BaseEntity<int>
    {
    }
}
