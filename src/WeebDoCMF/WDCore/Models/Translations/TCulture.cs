using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeebDoCMF.WDCore.Models.Translations
{
    public class TCulture
    {
        public int TCultureId { get; set; }

        [Required]
        public string CultureCode { get; set; }

        public List<TResource> Resources { get; set; }
    }
}
