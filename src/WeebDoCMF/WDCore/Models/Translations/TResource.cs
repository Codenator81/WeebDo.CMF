using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeebDoCMF.WDCore.Models.Translations
{
    public class TResource
    {
        public int TResourceId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }

        public int TCultureId { get; set; }
        public virtual TCulture Culture { get; set; }
    }
}
