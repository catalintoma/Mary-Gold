using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marigold
{
    public class Reservation
    {
        public string ReservationId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhoneNo { get; set; }

        [Required]
        public DateTime CheckinDate { get; set; }

        public bool CheckedOut { get; set; }

        public List<BillableService> BillableServices { get; set; }

        [Required]
        public string RoomDescription { get; set; }

        [Required]
        public int Units { get; set; }
    }
}