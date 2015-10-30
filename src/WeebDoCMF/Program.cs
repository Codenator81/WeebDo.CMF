using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime.Common.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using WeebDoCMF.Database.Seed;
using WeebDoCMF.WDCore.Models;

namespace WeebDoCMF.Commands.Lessy
{
    public class Program
    {
        public IConfiguration Configuration { get; set; }
        private readonly IServiceProvider _serviceProvider;

        public Program()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            
                    var appEnv = CallContextServiceLocator.Locator.ServiceProvider
                            .GetRequiredService<IApplicationEnvironment>();
                    ///defoult db file path /Database/weebdoDb.db
                    services.AddEntityFramework()
                       .AddSqlite()
                       .AddDbContext<MainDbContext>(options =>
                           options.UseSqlite($"Data Source=" + appEnv.ApplicationBasePath + Configuration["Data:dbSqlite:Path"]));

            
        }


        public int Main(string[] args)
        {
            BuildConfiguration();
            var app = new CommandLineApplication
            {
                Name = "dnx lessy",
                Description = "Your patner in web building",
                FullName = "Lessy - Less code more Fun"
            };
            app.VersionOption(
                "--version",
                Configuration["version"]);
            app.HelpOption("-?|-h|--help");
            app.Command("db:seed", c =>
            {
                c.Description = "Seed data to DB.";
                c.HelpOption("-?|-h|--help");
                c.OnExecute(() =>
                {
                    Console.WriteLine("Seeding data");
                    var res = SeedLanguages.AddData(_serviceProvider);       
                    if (res) {         
                        Console.WriteLine("Data saved");
                    } else
                    {
                        Console.WriteLine("Data already exist");
                    }                   
                    return 0;
                });
            });
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 2;
            });

            return app.Execute(args);
        }
        private void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("Settings/config.json");

            Configuration = builder.Build();
        }
    }
}
