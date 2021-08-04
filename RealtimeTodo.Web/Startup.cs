using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealtimeTodo.Web.Hubs;
using RealtimeToDo.Web.Services;

namespace RealtimeTodo.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(configure => {
                configure.RootPath = "wwwroot";
            });
            services.AddSingleton<IToDoRepository, InMemoryToDoRepository>();

            services.AddSignalR();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ToDoHub>("hubs/todo");
            });
            
            app.UseSpaStaticFiles();

            app.UseSpa(config => {
                config.UseProxyToSpaDevelopmentServer("http://localhost:8080");
            });
        }
    }
}
