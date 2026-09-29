using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Enum;

namespace Tourism.Core.Features.Booking.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }

        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; } // Ready / Custom

        public string? GuideId { get; set; }
        public string GuideName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public string Status { get; set; }
    }
}
