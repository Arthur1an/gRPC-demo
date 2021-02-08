using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public string UserName { get; set; }
    }
}
