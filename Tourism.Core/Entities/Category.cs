using System;
using System.Collections.Generic;
using System.Text;

namespace Tourism.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public string? TitleAr { get; set; }
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }


        public int Id { get; set; }

        public ICollection<Place> Places { get; set; } = new HashSet<Place>();
    }
}
