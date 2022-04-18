using Microsoft.AspNet.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparateControllers.Models
{
    [Select]
    public class LocalMarket: EntityBase
    { 
        public LocalMarket()
        {
        }

        public LocalMarket(int id)
        {
            Id = id;
        }

        public string Desc { get; set; }
    }
}
