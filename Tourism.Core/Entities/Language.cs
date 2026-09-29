using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; } 
        public ICollection<GuideLanguage> GuideLanguages { get; set; } = new HashSet<GuideLanguage>();
    }
}
