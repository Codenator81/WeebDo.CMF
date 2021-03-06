﻿using Microsoft.Data.Entity;
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
            //initialize configuration
            var builder = new ConfigurationBuilder()
                .AddJsonFile("Settings/config.json");
            Configuration = builder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {

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
                            options.UseSqlServer(Configuration["Data:dbSqlServer:ConnectionString"]));
                    break;
            }
        }


        public int Main(string[] args)
        {
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
    }
}
