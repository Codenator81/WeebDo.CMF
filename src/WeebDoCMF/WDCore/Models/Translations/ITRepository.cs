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
    }
}
