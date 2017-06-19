using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marigold
{
    public class ReservationsBll : IReservationsBll
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Reservation> _reservationRepository;

        private readonly IUnitOfWork<MarigoldDbContext> _uow;

        public ReservationsBll(IUnitOfWork<MarigoldDbContext> uow)
        {
            _uow = uow;

            _serviceRepository = uow.GetRepository<Service>();
            _unitRepository = uow.GetRepository<Unit>();
            _roomRepository = uow.GetRepository<Room>();
            _reservationRepository = uow.GetRepository<Reservation>();
        }

        public StaticPagedList<ReservationOutputDto> Reservations(int page, int pageSize)
        {
            return _reservationRepository.AdaptPaged(r => Mapper.Map<ReservationOutputDto>(r), page, pageSize);
        }

        public async Task<string> RoomUnitDescription()
        {
            return Mapper.Map<string>(await _roomRepository.RoomUnit());
        }

        public async Task<ReservationInputDto> ReservationInput()
        {
            var allServices = await _serviceRepository.All(include: r => r.Include(s => s.Unit));

            var rooms = allServices.Where(s => s is Room).Cast<Room>();
            var services = allServices.Where(s => !(s is Room) && !s.Extra);

            return new ReservationInputDto
            {
                Services = Mapper.Map<List<BillableServiceInputDto>>(services),
                Rooms = Mapper.Map<List<SelectListItem>>(rooms)
            };
        }

        public async Task Create(ReservationInputDto input)
        {
            var room = await _roomRepository.FindAsync(input.SelectedRoomId);

            var reservation = Mapper.Map<Reservation>(input);

            reservation.RoomDescription = room.Name;

            await _reservationRepository.InsertAsync(reservation);

            await _uow.SaveChangesAsync();
        }

        public async Task<ReservationInputDto> CheckoutView(string id)
        {
            var reservation = await _reservationRepository.FindAsync(id);

            if (reservation == null)
                return null;
            
            //get extra services
            var extraServices = await _serviceRepository.All(s => !(s is Room) && s.Extra, include: r => r.Include(s => s.Unit));

            var input = Mapper.Map<ReservationInputDto>(reservation);
            input.Services = Mapper.Map<List<BillableServiceInputDto>>(extraServices);

            return input;
        }

        public async Task<bool> Checkout(ReservationInputDto input)
        {
            if (string.IsNullOrEmpty(input.ReservationId)) if (string.IsNullOrEmpty(input.ReservationId))
                    return false;

            var reservation = await _reservationRepository.FindAsync(input.ReservationId);

            if (reservation == null)
                return false;
            
            //add chosen services
            input.Services
                .Where(s => s.Enabled).ToList()
                .ForEach(s => reservation.BillableServices.Add(Mapper.Map<BillableService>(s)));

            reservation.CheckedOut = true;

            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<BillOutputDto> Bill(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            //get reservation + child relationships
            var reservation = (await _reservationRepository.GetPagedListAsync(r => r.ReservationId == id, pageSize: 1,
                include: i => i.Include(r => r.BillableServices).ThenInclude(s => s.Service).ThenInclude(s => s.Unit))).Items.First();

            if (reservation == null)
                return null;

            var services = Mapper.Map<List<BillableServiceOutputDto>>(reservation.BillableServices);

           return new BillOutputDto
            {
                Services = services,
                CustomerName = reservation.CustomerName,
                CheckinDate = reservation.CheckinDate,
                Total = Mapper.Map<int>(services)
            };
        }
    }
}
