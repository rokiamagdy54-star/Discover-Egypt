using System.ComponentModel.DataAnnotations;
using Tourism.Core.Enum;

namespace Tourism.Core.Features.Authentication.DTOs
{
    public class GuideLanguageDto
    {
        [Required]
        public int LanguageId { get; set; }

        [Required]
        public LanguageLevel Level { get; set; }
    }
}