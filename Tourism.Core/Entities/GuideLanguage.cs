using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Enum;

namespace Tourism.Core.Entities
{
    public class GuideLanguage
    {
        public string GuideId { get; set; }
        public GuideProfile Guide { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public LanguageLevel Level { get; set; }
    }
}
