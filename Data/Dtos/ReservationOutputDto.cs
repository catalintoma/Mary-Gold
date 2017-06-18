using System;

namespace Marigold
{
    public class ReservationOutputDto
    {
        public string ReservationId { get; set; }

        public string RoomDescription { get; set; }

        public string CustomerName { get; set; }


        public DateTime CheckinDate { get; set; }

        public int Units { get; set; }

        public bool CheckedOut { get; set; }
    }
}