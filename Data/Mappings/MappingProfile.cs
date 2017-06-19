using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marigold
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Service, BillableServiceInputDto>()
                .ForMember(s => s.Unitless, opt => opt.MapFrom(s => s.Unitless))
                .ForMember(s => s.UnitDescription, opt => opt.MapFrom(s => s.Unit));

            CreateMap<BillableServiceInputDto,Service>();

            CreateMap<Room, SelectListItem>()
                .ForMember(s => s.Value, opt => opt.MapFrom(r => r.ServiceId))
                .ForMember(s => s.Text, opt => opt.MapFrom(r => r.Name));

            CreateMap<ReservationInputDto, Reservation>()
                .ForMember(r => r.Units, opt => opt.MapFrom(
                    i => i.CheckoutDate.Subtract(i.CheckinDate).Days
                ))
                .ForMember(r => r.BillableServices, opt => opt.MapFrom(
                     i => i.Services
                        .Where(s => s.Enabled)
                        .Select(s => Mapper.Map<BillableService>(s))
                        .Concat(new[]{new BillableService()
                        {
                            ServiceId = i.SelectedRoomId,
                            Units = i.CheckoutDate.Subtract(i.CheckinDate).Days
                        }})
                 ));

            CreateMap<BillableService, BillableServiceOutputDto>()
                .ForMember(o => o.Name, opt => opt.MapFrom(s => s.Service.Name))
                .ForMember(o => o.UnitDescription, opt => opt.MapFrom(s => Mapper.Map<string>(s.Service.Unit)))
                .ForMember(o => o.UnitPrice, opt => opt.MapFrom(s => s.Service.ExtraCustomPrice ? s.Units : s.Service.UnitPrice))
                .ForMember(o => o.Units, opt => opt.MapFrom(s => s.Service.ExtraCustomPrice ? 1 : s.Units));

            CreateMap<List<BillableService>, List<BillableServiceOutputDto>>()
                .ConstructUsing(l =>
                    l.OrderByDescending(s => s.Service, Comparer<Service>.Create((a, b) => (a is Room) ? 1 : -1))
                    .Select(s => Mapper.Map<BillableServiceOutputDto>(s))
                    .ToList());

            CreateMap<List<BillableServiceOutputDto>, int>()
                .ConstructUsing(l => l.Sum(s => s.Total));


            CreateMap<BillableServiceInputDto, BillableService>();
            CreateMap<Reservation, ReservationOutputDto>();
            CreateMap<Reservation, ReservationInputDto>();
            CreateMap<ServiceInputDto,Service>();

            CreateMap<Unit, string>()
                .ConvertUsing(u => u.Unitless ? string.Empty : $"{u.Name}(s)");



        }
    }
}