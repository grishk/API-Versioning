namespace ODataRuntime.Models
{
    public abstract class BaseEntity<T>
    {
        public T Key { get; set; }
        public string Name { get; set; }
    }
}
