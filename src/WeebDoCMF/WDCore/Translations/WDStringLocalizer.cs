using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.WDCore.Translations
{
    public class WDStringLocalizer : IStringLocalizer
    {
        private readonly ITRepository _tRepository;
        public WDStringLocalizer(ITRepository translationRepository)
        {
            _tRepository = translationRepository;
        }
        public LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var value = GetResourceFromDb(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                var format = GetResourceFromDb(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected string GetResourceFromDb(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _tRepository.GetResource(name);            
        }

    }
}
