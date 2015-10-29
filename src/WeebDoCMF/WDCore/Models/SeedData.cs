using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.WDCore.Models
{
    public class SeedData
    {
        public static async Task InitializeWeebDoCMFDatabaseAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<MainDbContext>();
            if (!context.TCultures.Any())
            {
                await CreateAdminUser(serviceProvider);
                await InsertCultureData(serviceProvider);
            }
        }
        private static async Task InsertCultureData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<MainDbContext>();
            if (!context.TCultures.Any())
            {
                var englishTwoLetterCulture = context.TCultures.Add(
                     new TCulture { CultureCode = "en", Name = "English" }).Entity;
                var russianTwoLetterCulture = context.TCultures.Add(
                    new TCulture { CultureCode = "ru", Name = "Русский" }).Entity;

                context.TResources.AddRange(
                      new TResource()
                      {
                          Name = "language",
                          Value = "Язык",
                          Culture = russianTwoLetterCulture
                      },
                     new TResource()
                     {
                         Name = "language",
                         Value = "Language",
                         Culture = englishTwoLetterCulture
                     }
                );
                await context.SaveChangesAsync();
            }
        }


        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetService<IOptions<AppSettings>>().Value;
            var adminRole = settings.adminRole;

            var userManager = serviceProvider.GetService<UserManager<WeebDoCmfUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var user = await userManager.FindByNameAsync(settings.adminName);
            if (user == null)
            {
                user = new WeebDoCmfUser { UserName = settings.adminName };
                await userManager.CreateAsync(user, settings.adminPassword);
                await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageAdminPanel", "Allowed"));
            }
        }
    }
}
