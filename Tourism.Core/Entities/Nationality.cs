using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Nationality : BaseEntity
    {
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    }
}
