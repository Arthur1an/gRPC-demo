using Grpc.Core;
using leaveDemo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcGreeter.Entity;
using System.ComponentModel;
using System.Reflection;

namespace LeaveDemo
{
    public class LeaveServices:leave.leaveBase
    {
        private readonly ILogger<LeaveServices> _logger;
        private readonly GrpcGreeter.Entity.LeaveDbContext _dbContext;
        public LeaveServices(GrpcGreeter.Entity.LeaveDbContext dbContext, ILogger<LeaveServices> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request, ServerCallContext context) {
            var Personnel = _dbContext.Personnels.All(x => x.Name == request.Name);
            var list = (from o in _dbContext.Personnels where o.Name == request.Name 
                       select o).ToList();
            var ApplicationLeave = (from o in _dbContext.ApplicationLeaves
                                    where o.Applicant.Personnel.Name == request.Name
                                    select o).ToList();
            var user = new GetUserInfoResponse
            {
                Name = "小明",
                Age = "22",
                Gender = Gender.Man.ToString(),
                Phone= "1996XXX",
                Email="XXX"
            };
            //var arr = ConvertObject(ApplicationLeave, user.GetType());
            return Task.FromResult(user);
        }
        public override Task<GetUserInfoListResponse> GetUserInfoList(GetUserInfoRequest request, ServerCallContext context)
        {
            //var Personnel = _dbContext.Personnels.All(x => x.Name == request.Name);
            var list = (from o in _dbContext.Personnels
                        select o).ToList();
            GetUserInfoListResponse rtn = new GetUserInfoListResponse();
            //rtn.Messages.Add(new List
            //{
            //    Name = "小明",
            //    StartDate = DateTime.Now.ToShortDateString(),
            //    EndDate = DateTime.Now.AddDays(1).ToShortDateString(),
            //    LeaveType = "MaternityLeave"
            //});
            //rtn.Messages.Add(new List
            //{
            //    Name = "小红",
            //    StartDate = DateTime.Now.ToShortDateString(),
            //    EndDate = DateTime.Now.AddDays(2).ToShortDateString(),
            //    LeaveType = "MaternityLeave"
            //});
            foreach (var item in list)
            {
                var ApplicationLeave = _dbContext.ApplicationLeaves.FirstOrDefault(x => x.Applicant.Personnel.Id == item.Id);
                rtn.Messages.Add(new List
                {
                    Name = item.Name,// "小明",
                    StartDate = ApplicationLeave.StartDate.ToString(),//  DateTime.Now.ToShortDateString(),
                    EndDate = ApplicationLeave.EndDate.ToString(),//DateTime.Now.AddDays(1).ToShortDateString(),
                    LeaveType = ApplicationLeave.LeaveType.ToString()//"MaternityLeave"
                });
            }
            return Task.FromResult(rtn);
        }
        public override Task<ResponseMessage> Apply(ApplyRequest request, ServerCallContext context) {
            
            Employee emp = new Employee();
            var Personnel = _dbContext.Personnels.FirstOrDefault(x => x.Name == request.Name);
            //var Applicant = _dbContext.ApplicationLeaves.FirstOrDefault(x => x.Applicant.Personnel.Name == request.Name);
            if (Personnel == null)
            {
                Personnel = new Personnel();
                Personnel.Name = request.Name;
                Personnel.Age = 18;
                Personnel.Gender = Gender.Man;
                emp.Personnel = Personnel;
                emp.FirstWorkDate = DateTime.Now;
                _dbContext.Add(Personnel);
                _dbContext.Add(emp);
                _dbContext.SaveChanges();
                //return Task.FromResult(new ResponseMessage { Message = "员工" + request.Name + "不存在" });
            }
            //判断该种类假期是否有剩余
            GrpcGreeter.Entity.ApplicationLeave ApplicationLeaves = new GrpcGreeter.Entity.ApplicationLeave();
            ApplicationLeaves.Applicant = emp;
            ApplicationLeaves.Approver = emp;
            ApplicationLeaves.StartDate = DateTime.Parse(request.StartDate);
            ApplicationLeaves.EndDate = DateTime.Parse(request.EndDate);
            ApplicationLeaves.LeaveType = ApplicationLeave.LeaveTypes.MaternityLeave;
            ApplicationLeaves.isAudit = false;
            _dbContext.Add(ApplicationLeaves);
            _dbContext.SaveChanges();

            return Task.FromResult(new ResponseMessage { Message = "提交成功" });
        }

        public override Task<ResponseMessage> Approve(ApproveRequest request, ServerCallContext context) {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(x => x.Personnel.Name == request.Approve);
                if (employee == null)
                {
                    return Task.FromResult(new ResponseMessage { Message = "审核失败，"+request.Approve+"" });
                }
                var leave = _dbContext.ApplicationLeaves.FirstOrDefault(x => x.Applicant.Personnel.Name == request.Apply);
                leave.isAudit = true;
                leave.Approver = employee;
                _dbContext.SaveChanges();
                return Task.FromResult(new ResponseMessage { Message = "审核成功" });
            }
            catch (Exception)
            {
                return Task.FromResult(new ResponseMessage { Message = "审核失败，员工信息错误" });
            }
        }

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>转换后的对象</returns>
        //private object ConvertObject(object obj, Type type)
        //{
        //    if (type == null) return obj;
        //    if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

        //    Type underlyingType = Nullable.GetUnderlyingType(type);
        //    if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
        //    {
        //        return obj;
        //    }
        //    else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
        //    {
        //        if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return Enum.Parse(underlyingType ?? type, obj.ToString());
        //        }
        //    }
        //    else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
        //    {
        //        try
        //        {
        //            return Convert.ChangeType(obj, underlyingType ?? type, null);
        //        }
        //        catch
        //        {
        //            return underlyingType == null ? Activator.CreateInstance(type) : null;
        //        }
        //    }
        //    else
        //    {
        //        TypeConverter converter = TypeDescriptor.GetConverter(type);
        //        if (converter.CanConvertFrom(obj.GetType()))
        //        {
        //            return converter.ConvertFrom(obj);
        //        }
        //        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
        //        if (constructor != null)
        //        {
        //            object o = constructor.Invoke(null);
        //            PropertyInfo[] propertys = type.GetProperties();
        //            Type oldType = obj.GetType();
        //            foreach (PropertyInfo property in propertys)
        //            {
        //                PropertyInfo p = oldType.GetProperty(property.Name);
        //                if (property.CanWrite && p != null && p.CanRead)
        //                {
        //                    property.SetValue(o, ConvertObject(p.GetValue(obj, null), property.PropertyType), null);
        //                }
        //            }
        //            return o;
        //        }
        //    }
        //    return obj;
        //}


    }
}
