using ODataRuntime.Models;

namespace ODataRuntime.Impl.Models
{
    public class Site: EntityKeyInt
    {
        public string Administrator { get; set; }
        public int Counter { get; set; }
    }
}
