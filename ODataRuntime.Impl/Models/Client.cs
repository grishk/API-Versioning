using ODataRuntime.Models;
using System;

namespace ODataRuntime.Impl.Models
{
    public class Client: EntityKeyInt
    {
        public DateTime Created { get; set; }
        public bool IsOwner { get; set; }
        public decimal Fee { get; set; }
    }
}
