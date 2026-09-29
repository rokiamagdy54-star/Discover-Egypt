using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Enums;

namespace Tourism.Core.Features.Booking.DTOs
{
    public class CreateBookingDto
    {
        public int PlanId { get; set; }
        public string? GuideId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public bool UsePoints { get; set; }
    }
}
