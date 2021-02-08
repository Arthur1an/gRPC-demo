using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    public class Stock:BaseEntity
    {
        public string Name { get; set; }
        public int? Code { get; set; }
    }
}
