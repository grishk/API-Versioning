using System;
using ODataRuntime.Models;

namespace ODataRuntime.Impl.Models
{
    public class Manager : EntityKeyInt
    {
        public string Position { get; set; }
        public double Salary { get; set; }
    }
}