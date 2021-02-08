using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Entity
{
    public class LeaveDbContext:DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options)
        {
        }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ApplicationLeave> ApplicationLeaves { get; set; }

        public DbSet<Stock> Stock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationLeave>().HasOne(x => x.Applicant).WithMany(x => x.Applicants);
            modelBuilder.Entity<ApplicationLeave>().HasOne(x => x.Approver).WithMany(x => x.Approvers);

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //  => optionsBuilder.UseNpgsql("Host=localhost;Database=LeaveDemo;Username=postgres;Password=1qaz");
    }
}
