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
            CreateMap<Service, ServiceInputDto>()
                .ForMember(s => s.Unitless, opt => opt.MapFrom(s => s.Unitless))
                .ForMember(s => s.UnitDescription, opt => opt.MapFrom(s => $"{s.Unit.Name}(s)"));

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
                        .Select(s => new BillableService()
                        {
                            ServiceId = s.ServiceId,
                            Units = s.Units
                        })
                        .Concat(new[]{new BillableService()
                        {
                            ServiceId = i.SelectedRoomId,
                            Units = i.CheckoutDate.Subtract(i.CheckinDate).Days
                        }})
                 ));

            CreateMap<Reservation, ReservationOutputDto>();

            CreateMap<Unit, string>()
                .ConvertUsing(u => $"{u.Name}s");



        }
    }
}