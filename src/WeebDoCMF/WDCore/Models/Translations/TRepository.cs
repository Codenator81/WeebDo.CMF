using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WeebDoCMF.WDCore.Models.Translations
{
    public class TRepository : ITRepository
    {
        private readonly MainDbContext _dbContext;

        public TRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetResource(string name)
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            try
            {
                var tResource = _dbContext.TResources
                    .Include(c => c.Culture)
                    .SingleOrDefault(r => r.Name == name && r.Culture.CultureCode == currentCulture);
                if (tResource == null)
                {
                    return name;
                }
                return tResource.Value;
            }
            catch (Exception)
            {
                return name;
            }
        }
        public IList<TCulture> GetCulturesAll()
        {
            return _dbContext.TCultures.Distinct().ToList();
        }

        public string GetCultureByCode(string name)
        {
            try {
                var culture = _dbContext.TCultures.SingleOrDefault(c => c.CultureCode == name);
                return culture.CultureCode;
            } catch(Exception)
            {
                return null;
            }
        }
    }
}
