using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeebDoCMF.WDCore.Models.Translations
{
    public interface ITRepository
    {
        /// <summary>
        /// Return translation value from DB or if not find name passed in 
        /// </summary>
        /// <param name="name">
        /// Key to search from
        /// </param>
        /// <returns>
        /// translation string or name
        /// </returns>
        string GetResource(string name);

        /// <summary>
        /// Return CultureCode from DB if exist or null
        /// </summary>
        /// <param name="name">Culture code</param>
        /// <returns></returns>
        string GetCultureByCode(string name);
    }
}
