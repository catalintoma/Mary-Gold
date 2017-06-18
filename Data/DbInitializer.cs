using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Marigold
{
    public static class DbInitializer
    {
        public static async void Initialize(IUnitOfWork<MarigoldDbContext> uow)
        {
            //ensure db created
            uow.DbContext.Database.EnsureCreated();

            var unitRepo = uow.GetRepository<Unit>();
            var roomRepo = uow.GetRepository<Room>();
            var servicesRepo = uow.GetRepository<Service>();

            //store some units,in case we need them later
            Unit roomUnit, unitlessUnit, dayunit = null;

            #region Seed units

            if (!(await uow.DbContext.Units.AnyAsync()))
            {
                var units = new[]{
                    Unit.Named("Pc"),
                    Unit.Named("Day"),
                    Unit.Named("Night"),
                    Unit.Named("Hour"),
                    Unit.Named(Unit.UnitlessKey)
                };

                roomUnit = units.First(u => u.Name == "Night");
                dayunit = units.First(u => u.Name == "Day");
                unitlessUnit = units.First(u => u.Name == Unit.UnitlessKey);

                await unitRepo.InsertAsync(units);
            }
            else
            {
                roomUnit = (await unitRepo.GetPagedListAsync(u => u.Name == "Night", pageSize: 1)).Items.First();
                dayunit = (await unitRepo.GetPagedListAsync(u => u.Name == "Day", pageSize: 1)).Items.First();
                unitlessUnit = (await unitRepo.GetPagedListAsync(u => u.Name == Unit.UnitlessKey, pageSize: 1)).Items.First();
            }

            #endregion Seed units

            #region Seed rooms

            if (!(await uow.DbContext.Rooms.AnyAsync()))
            {
                await roomRepo.InsertAsync(new[]{
                    new ServiceBuilder<Room>()
                        .Named("Single room")
                        .WithUnit(roomUnit)
                        .Priced(101)
                        .Create(),
                    new ServiceBuilder<Room>()
                        .Named("Standard room")
                        .WithUnit(roomUnit)
                        .Priced(404)
                        .Create(),
                    new ServiceBuilder<Room>()
                        .Named("High suite room")
                        .WithUnit(roomUnit)
                        .Priced(420)
                        .Create()
                });
            }

            #endregion Seed rooms

            #region Seed services

            if (!(await uow.DbContext.Services.AnyAsync()))
            {
                await servicesRepo.InsertAsync(new[]{
                    new ServiceBuilder<Service>()
                        .Named("Wifi")
                        .WithUnit(unitlessUnit)
                        .Priced(101)
                        .Create(),
                    new ServiceBuilder<Service>()
                        .Named("Parking")
                        .WithUnit(unitlessUnit)
                        .Priced(101)
                        .Create(),
                    new ServiceBuilder<Service>()
                        .Named("Bike")
                        .WithUnit(dayunit)
                        .Priced(101)
                        .Create(),
                    new ServiceBuilder<Service>()
                        .Named("Minibar")
                        .ExtraCustomPrice(unitlessUnit)
                        .Create(),
                    new ServiceBuilder<Service>()
                        .Named("Extra Charges")
                        .ExtraCustomPrice(unitlessUnit)
                        .Create()
                });
            }

            #endregion Seed services

            //commit unit of work
            await uow.SaveChangesAsync();
        }
    }
}