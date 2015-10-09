using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WeebDoCMF.Core.Models;
using WeebDoCMF.Settings;

namespace WeebDoCMF.Areas.WDCore.Models
{
    public class SampleData
    {
        public static async Task InitializeWeebDoCMFDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<MainDbContext>();
                if (await db.Database.EnsureCreatedAsync())
                {
                    await CreateAdminUser(serviceProvider);
                }
            }
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetService<IOptions<AppSettings>>().Value;
            const string adminRole = "Administrator";

            var userManager = serviceProvider.GetService<UserManager<WeebDoCmsUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var user = await userManager.FindByNameAsync(settings.adminName);
            if (user == null)
            {
                user = new WeebDoCmsUser { UserName = settings.adminName };
                await userManager.CreateAsync(user, settings.adminPassword);
                await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageAdminPanel", "Allowed"));
            }
        }
    }
}
