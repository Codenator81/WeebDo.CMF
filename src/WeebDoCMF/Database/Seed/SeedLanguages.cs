using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WeebDoCMF.WDCore.Models;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.Database.Seed
{
     public class SeedLanguages
    {
        public static bool AddData(IServiceProvider _serviceProvider)
        {
            // DbContext initialization
            var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            if (!dbContext.TCultures.Any())
            {
                var englishTwoLetterCulture = dbContext.TCultures.Add(
                     new TCulture { CultureCode = "en", Name = "English" }).Entity;
                var russianTwoLetterCulture = dbContext.TCultures.Add(
                    new TCulture { CultureCode = "ru", Name = "Русский" }).Entity;

                dbContext.TResources.AddRange(
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
                dbContext.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }
    }
}
