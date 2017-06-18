using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Marigold
{
    public class ReservationsController : Controller
    {
        private IUnitOfWork unitOfWork;

        private readonly IRepository<Service> serviceRepository;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<Room> roomRepository;

        public ReservationsController(IUnitOfWork uow)
        {
            unitOfWork = uow;
            serviceRepository = uow.GetRepository<Service>();
            reservationRepository = uow.GetRepository<Reservation>();
            roomRepository = uow.GetRepository<Room>();
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageIndex = !page.HasValue || page <= 0 ? 1 : page.Value;
            var roomUnit = await roomRepository.RoomUnit();
            var roomUnitDescription = Mapper.Map<string>(roomUnit);

            var list = reservationRepository.AdaptPaged(r => Mapper.Map<ReservationOutputDto>(r), pageIndex - 1, 1);

            return View(new ReservationsViewModel
            {
                Reservations = list,
                RoomUnitDescription = roomUnitDescription
            });
        }

        public async Task<IActionResult> Create()
        {
            //we don't query by service type since we use both services and rooms
            var allServices = await serviceRepository.All(include: r => r.Include(s => s.Unit));

            var rooms = allServices.Where(s => s is Room).Cast<Room>();
            var services = allServices.Where(s => !(s is Room));

            return View(new ReservationInputDto
            {
                Services = Mapper.Map<List<ServiceInputDto>>(services),
                Rooms = Mapper.Map<List<SelectListItem>>(rooms)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationInputDto input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var room = await roomRepository.FindAsync(input.SelectedRoomId);

            var reservation = Mapper.Map<Reservation>(input);

            reservation.RoomDescription = room.Name;

            await reservationRepository.InsertAsync(reservation);

            await unitOfWork.SaveChangesAsync();

            return Redirect("Index");
        }
    }
}