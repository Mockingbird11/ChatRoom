using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChatRoom.DataAcces.Redis;
using Microsoft.AspNetCore.Mvc;
using log4net.Repository;
using log4net;
using System.IO;
using log4net.Config;
using ChatRoom.DataAcces.Log4Net;
using CharRoom.Entity.CommonModels;
using ChatRoom.Services.Application;
using ChatRoom.IServices;
using ChatRoom.Areas.ChatCenter.Services;

namespace ChatRoom
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Logger());
            //将Redis添加进管道
            services.AddSingleton(new RedisHelper());
            //注入自定义服务组件
            services.AddScoped<IAppLoginService, AppLoginService>();
            //设置session过期时间为24小时
            services.AddHttpContextAccessor();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromHours(24);
            });
            
            services.AddControllersWithViews();
            //不启用终结点
            services.AddMvc(options => options.EnableEndpointRouting = false);


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
                ReceiveBufferSize = 1 * 1024
            });
            app.UseMiddleware<WebsocketHandlerMiddleware>();
            
            app.UseHttpsRedirection();
            //使用静态文件中间件
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Users}/{action=LoginIndex}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}",
                //    defaults: new { controller = "Login", action = "Index" });
            });

        }
    }
}
