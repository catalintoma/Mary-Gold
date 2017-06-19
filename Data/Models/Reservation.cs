using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marigold
{
    public class Reservation
    {
        public Reservation()
        {
            BillableServices = new List<BillableService>();
        }
        public string ReservationId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhoneNo { get; set; }

        [Required]
        public DateTime CheckinDate { get; set; }

        public bool CheckedOut { get; set; }

        public List<BillableService> BillableServices { get; set; }

        //Both this properties can be inferred by searching the room in the list of billable services
        //Stored for brevity
        [Required]
        public string RoomDescription { get; set; }

        [Required]
        public int Units { get; set; }
    }
}