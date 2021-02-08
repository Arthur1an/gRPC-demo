using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    /// <summary>
    /// 雇员
    /// </summary>
    public class Employee:BaseEntity
    {
        
        public Personnel Personnel { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public int PersonnelId { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime? EntryDate { get; set; }
        /// <summary>
        /// 工作日期
        /// </summary>
        public DateTime? FirstWorkDate { get; set; }

        public ICollection<ApplicationLeave> Applicants { get; set; } = new HashSet<ApplicationLeave>();

        public ICollection<ApplicationLeave> Approvers { get; set; } = new HashSet<ApplicationLeave>();



    }
 
}
