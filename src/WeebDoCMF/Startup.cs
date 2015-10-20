using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Localization;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WeebDoCMF.WDCore.Middleware;
using WeebDoCMF.WDCore.Models;
using WeebDoCMF.WDCore.Models.Translations;
using WeebDoCMF.WDCore.Translation;

namespace WeebDoCMF
{
    public class Startup
    {
        public Startup(IApplicationEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddJsonFile("Settings/config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

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

                                    services.AddEntityFramework()
                                       .AddNpgsql()
                                       .AddDbContext<MainDbContext>(options =>
                                           options.UseNpgsql(connectPostgres));
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
            services.AddIdentity<WeebDoCmfUser, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath = "/wdadmin/Account/Login";
            })
                .AddEntityFrameworkStores<MainDbContext>()
                .AddDefaultTokenProviders();


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://example.com");
                });
            });
            services.AddLocalization();
            // Add MVC services to the services container.
            services.AddMvc()
                .AddViewLocalization()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            // Add memory cache services
            services.AddCaching();

            // Add session related services.
            services.AddSession();

            //add services 
            // Example: services.AddTransient<ITestService, TestService>();   
            services.AddScoped<ITRepository, TRepository>();
            services.AddTransient<IStringLocalizerFactory, WDStringLocalizerFactory>();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageAdminPanel",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageAdminPanel", "Allowed");
                    });
            });
        }

        //This method is invoked when ASPNET_ENV is 'Development' or is not defined
        //The allowed values are Development,Staging and Production
        public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);

            // StatusCode pages to gracefully handle status codes 400-599.
            //app.UseStatusCodePagesWithRedirects("/Home/StatusCodePage");
            app.UseStatusCodePages();

            // Display custom error page in production when error occurs
            // During development use the ErrorPage middleware to display error information in the browser
            app.UseDeveloperExceptionPage();

            app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);

            // Add the runtime information page that can be used by developers
            // to see what packages are used by the application
            // default path is: /runtimeinfo
            app.UseRuntimeInfoPage();

            Configure(app);
        }

        //This method is invoked when ASPNET_ENV is 'Production'
        //The allowed values are Development,Staging and Production
        public void ConfigureProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);

            // StatusCode pages to gracefully handle status codes 400-599.
            //app.UseStatusCodePagesWithRedirects("/Home/StatusCodePage");

            app.UseExceptionHandler("/Home/Error");

            Configure(app);
        }


        public void Configure(IApplicationBuilder app)
        {
            // Configure Session.
            app.UseSession();

            // Add cookie auth
            app.UseIdentity();

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU")
                }

            };
            options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            {
                var segments = context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length >= 1 && (segments[0].Length == 5 || segments[0].Length == 2))
                {
                    var cultures = options.SupportedCultures;
                    var pathCulture = new CultureInfo(segments[0]);
                    if (cultures.Contains(pathCulture))
                    {
                        var requestCulture = new RequestCulture(pathCulture);
                        return await Task.FromResult(requestCulture);
                    }                    
                }
                return null;
            }));

            app.UseRequestLocalization(options);
            // Configure Session.
            app.UseProtectFolder(new ProtectFolderOptions
            {
                Path = "/Protected",
                RoleName = Configuration["AppSettings:adminRole"]
            });

            // Add static files
            app.UseStaticFiles();

            // Add MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Admin", action = "Index" });
                routes.MapRoute(
                    name: "transRoute",
                    template: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");                    
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
        });

            // Initialize the sample data
            //Uncomment on first run after do all migrations
            //SampleData.InitializeWeebDoCMFDatabaseAsync(app.ApplicationServices).Wait();
        }
}
}
