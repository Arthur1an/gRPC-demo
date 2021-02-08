using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    /// <summary>
    /// 申请/审批记录
    /// </summary>
    public class ApplicationLeave:BaseEntity
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public int ApplicantId { get; set; }
        public Employee Applicant { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        public int ApproverId { get; set; }
        public Employee Approver { get; set; }
        /// <summary>
        /// 请假类型
        /// </summary>
        public LeaveTypes LeaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool isAudit { get; set; }

        public enum LeaveTypes
        {
            /// <summary>
            /// 年假
            /// </summary>
            AnnualLeave,
            /// <summary>
            /// 事假
            /// </summary>
            MaternityLeave
        }
    }
}
