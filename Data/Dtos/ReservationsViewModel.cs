using PagedList.Core;

namespace Marigold
{
    public class ReservationsViewModel
    {
        public StaticPagedList<ReservationOutputDto> Reservations { get; set; }

        public string RoomUnitDescription { get; set; }
    }
}