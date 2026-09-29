using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Entities
{
    public class TouristProfile
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Points { get; set; } = 0;
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<PlaceReview> PlaceReviews { get; set; } = new HashSet<PlaceReview>();
        public ICollection<GuideReview> GuideReviews { get; set; } = new HashSet<GuideReview>();
        public ICollection<CommunityPost> CommunityPosts { get; set; } = new HashSet<CommunityPost>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Requset> BookingRequests { get; set; } = new HashSet<Requset>();
        public ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();
        public ICollection<CustomPlan> CustomPlans { get; set; } = new HashSet<CustomPlan>();
    }
}