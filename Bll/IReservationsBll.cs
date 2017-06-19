using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace Marigold
{
    public interface IReservationsBll
    {
        StaticPagedList<ReservationOutputDto> Reservations(int page, int pageSize);

        Task<string> RoomUnitDescription();

        Task<ReservationInputDto> ReservationInput();

        Task Create(ReservationInputDto input);

        Task<ReservationInputDto> CheckoutView(string id);

        Task<bool> Checkout(ReservationInputDto input);

        Task<BillOutputDto> Bill(string id);
    }
}