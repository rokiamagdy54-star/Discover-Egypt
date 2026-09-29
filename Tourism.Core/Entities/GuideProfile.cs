using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tourism.Core.Enum;

namespace Tourism.Core.Entities
{
    public class GuideProfile
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string LicenseNumber { get; set; }
        public string LicenseImageUrl { get; set; }

        public GuideStatus Status { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<GuideReview> GuideReviews { get; set; } = new HashSet<GuideReview>();
        public ICollection<ReadyPlan> ReadyPlans { get; set; } = new HashSet<ReadyPlan>();
        public ICollection<Requset> BookingRequests { get; set; } = new HashSet<Requset>();
        public ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();
        public ICollection<GuideLanguage> GuideLanguages { get; set; } = new HashSet<GuideLanguage>();
    }
}