using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    /// <summary>
    /// 人员
    /// </summary>
    public class Personnel:BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
    
    }
   
    public enum Gender { 
        Man,
        Female
    }
  
}
