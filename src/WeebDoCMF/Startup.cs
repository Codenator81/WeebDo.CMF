using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WeebDoCMF.Core.Models;

namespace WeebDoCMF
{
    public class Startup
    {
        public Startup(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
                Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC services to the services container.
            services.AddMvc();

            /// Database setting
            /// at this moment have 3 variant 
            /// Postgres(currently not work on core 5)
            /// Sqlite
            /// SqlServer
            switch (Configuration["Data:DbName"])
            {
/*
                case "Postgres":
                    var host = Configuration["Data:dbPostgres:Host"];
                    var username = Configuration["Data:dbPostgres:Username"];
                    var password = Configuration["Data:dbPostgres:Password"];
                    var database = Configuration["Data:dbPostgres:Database"];
                    var connectPostgres = host + username + password + database;
                   
#if DNX451
                    //TODO: remofe #if DNX451 when postgres EntityFramework support Core 5
                    services.AddEntityFramework()
                       .AddNpgsql()
                       .AddDbContext<MainDbContext>(options =>
                           options.UseNpgsql(connectPostgres));
#endif
                    break;
*/
                case "Sqlite":
                    var appEnv = CallContextServiceLocator.Locator.ServiceProvider
                            .GetRequiredService<IApplicationEnvironment>();
                    ///defoult db file path /Database/weebdoDb.db
                    services.AddEntityFramework()
                       .AddSqlite()
                       .AddDbContext<MainDbContext>(options =>
                           options.UseSqlite($"Data Source=" + appEnv.ApplicationBasePath + Configuration["Data:dbSqlite:Path"]));
                    break;
                default:
                    services.AddEntityFramework()
                        .AddSqlServer()
                        .AddDbContext<MainDbContext>(options =>
                            options.UseSqlServer(Configuration["dbSqlServer:ConnectionString"]));
                    break;                    
            }

            // Add Identity services to the services container.
            services.AddIdentity<WeebDoCmsUser, IdentityRole>()
                .AddEntityFrameworkStores<MainDbContext>()
                .AddDefaultTokenProviders();

            //add services 
            // Example: services.AddTransient<ITestService, TestService>();            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (Configuration["env"] == "dev")
            {
                
            }
            else
            {
                // Register how to generate response bodies for 400-599 status codes.
                // This example ends up using the MVC ErrorsController.
                // TODO Make controller and views for errors
                app.UseStatusCodePagesWithReExecute("/errors/{0}");
            }
            // Add cookie auth
            app.UseIdentity();

            // Add static files
            app.UseStaticFiles();

            // Add MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

            routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
