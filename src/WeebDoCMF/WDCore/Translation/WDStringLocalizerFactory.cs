using Microsoft.Extensions.Localization;
using System;
using WeebDoCMF.WDCore.Models;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.WDCore.Translation
{
    public class WDStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly MainDbContext _dbContext;
        public WDStringLocalizerFactory(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var translationRepository = new TRepository(_dbContext);
            return new WDStringLocalizer(
                translationRepository
            );
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var translationRepository = new TRepository(_dbContext);
            return new WDStringLocalizer(
                translationRepository
            );
        }
    }
}
