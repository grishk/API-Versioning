using Microsoft.AspNet.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparateControllers.Models
{
    [Select]
    public class GlobalMarket: LocalMarket
    { 
        public GlobalMarket()
        {
        }

        public GlobalMarket(int id)
            :base(id)
        {
        }

        public string GlobalDesc { get; set; }
    }
}
