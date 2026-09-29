using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Tourism.Core.Enum;

namespace Tourism.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }

        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public int NationalityId { get; set; }
        public Nationality Nationality { get; set; }

        public TouristProfile Tourist { get; set; }
        public GuideProfile Guide { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
    }

}
