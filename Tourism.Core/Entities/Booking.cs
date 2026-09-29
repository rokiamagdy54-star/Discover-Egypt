using System;
using Tourism.Core.Enum;
using Tourism.Core.Enums;

namespace Tourism.Core.Entities
{
    public class Booking : BaseEntity
    {
        public BookingStatus Status { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime BookingStart { get; set; }
        public DateTime BookingEnd { get; set; }
        public DateTime? CancelledAt { get; set; }
        public int NumberOfPeople { get; set; }
        public string? CancelReason { get; set; }
        public string TouristId { get; set; }
        public TouristProfile Tourist { get; set; }
        public int PlanId { get; set; }
        public BasePlan Plan { get; set; }
        public string? GuideId { get; set; }
        public GuideProfile? Guide { get; set; }
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}

