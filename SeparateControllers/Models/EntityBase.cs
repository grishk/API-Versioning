﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparateControllers.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
        }
        public EntityBase(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
