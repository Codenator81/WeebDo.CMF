using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using WeebDoCMS.Area.WeebDo.Model;

namespace WeebDoCMS
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
                Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCaching();
            services.AddSession();


            /// Database setting
            /// at this moment have 3 variant 
            /// Postgres
            /// Sqlite
            /// SqlServer
            switch (Configuration["Data:DbName"])
            {
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

            // Add MVC services to the services container.
            services.AddMvc();
            //After MVC add services 
            // Example: services.AddTransient<ITestService, TestService>();
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (Configuration["env"] == "dev")
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                //TODO: create default controller for error page
                app.UseStatusCodePagesWithRedirects("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseSession();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
