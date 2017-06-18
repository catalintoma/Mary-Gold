using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var roomUnitDescription = Mapper.Map<string>(await roomRepository.RoomUnit());

            var list = reservationRepository.AdaptPaged(r => Mapper.Map<ReservationOutputDto>(r), pageIndex - 1, 10);

            ViewBag.RoomUnitDescription = roomUnitDescription;
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            //we don't query by service type since we use both services and rooms
            var allServices = await serviceRepository.All(include: r => r.Include(s => s.Unit));

            var rooms = allServices.Where(s => s is Room).Cast<Room>();
            var services = allServices.Where(s => !(s is Room) && !s.Extra);

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

        public async Task<IActionResult> Checkout(string id)
        {
            var reservation = await reservationRepository.FindAsync(id);

            if (reservation == null)
                return new NotFoundResult();

            var roomUnitDescription = Mapper.Map<string>(await roomRepository.RoomUnit());

            var extraServices = await serviceRepository.All(s => !(s is Room) && s.Extra, include: r => r.Include(s => s.Unit));

            ViewBag.RoomUnitDescription = roomUnitDescription;

            var input = Mapper.Map<ReservationInputDto>(reservation);
            input.Services = Mapper.Map<List<ServiceInputDto>>(extraServices);

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ReservationInputDto input)
        {
            if (string.IsNullOrEmpty(input.ReservationId)) if (string.IsNullOrEmpty(input.ReservationId))
                    return new NotFoundResult();

            var reservation = await reservationRepository.FindAsync(input.ReservationId);

            if (reservation == null)
                return new NotFoundResult();

            input.Services
                .Where(s => s.Enabled).ToList()
                .ForEach(s => reservation.BillableServices.Add(Mapper.Map<BillableService>(s)));
                
            reservation.CheckedOut = true;

            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Bill(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new NotFoundResult();

            var reservation = (await reservationRepository.GetPagedListAsync(r => r.ReservationId == id, pageSize: 1,
                include: i => i.Include(r => r.BillableServices).ThenInclude(s => s.Service).ThenInclude(s => s.Unit))).Items.First();

            if (reservation == null)
                return new NotFoundResult();

            var services = Mapper.Map<List<BillableServiceOutputDto>>(reservation.BillableServices);
            
            var output = new BillOutputDto
            {
                Services = services,
                CustomerName = reservation.CustomerName,
                CheckinDate = reservation.CheckinDate,
                Total = Mapper.Map<int>(services)
            };

            ViewBag.Currency = "grams of gold";
            return View(output);
        }
    }
}