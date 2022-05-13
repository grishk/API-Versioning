using System;
using ODataRuntime.Models;

namespace ODataRuntime.Impl.Models
{
    public class Person : EntityKeyInt
    {
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}