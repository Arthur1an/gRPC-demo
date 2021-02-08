using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class AppSettings
    { 
        public string MyApiConection { get; set; }

    }
    
    public class Startup
    {
        private readonly IConfiguration  _configuration;
        public Startup(IConfiguration configuration) {

            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddControllersWithViews();
            services.AddGrpcReflection();
            var st = new AppSettings();
            var section = _configuration.GetSection("AppSettings");
            section.Bind(st);
            services.Configure<AppSettings>(section);


            services.AddDbContext<Entity.LeaveDbContext>(option =>
            {
                option.UseNpgsql(_configuration.GetConnectionString(st.MyApiConection));
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
            
                endpoints.MapGrpcService<StockServices.Services.StockService>().EnableGrpcWeb();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Accounts}/{action=Index}/{id?}");
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<GrpcTest.TestService>();
                endpoints.MapGrpcService<LeaveDemo.LeaveServices>();
                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
