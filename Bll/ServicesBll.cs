using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marigold
{
    public class ServicesBll : IServicesBll
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IUnitOfWork<MarigoldDbContext> _uow;

        public ServicesBll(IUnitOfWork<MarigoldDbContext> uow)
        {
            _uow = uow;
            _serviceRepository = uow.GetRepository<Service>();
            _unitRepository = uow.GetRepository<Unit>();
        }

        public async Task<IEnumerable<BillableServiceInputDto>> AllServices()
        {
            var allServices = await _serviceRepository.All(include: r => r.Include(s => s.Unit));

            return Mapper.Map<IEnumerable<BillableServiceInputDto>>(allServices);
        }

        public async Task<BillableServiceInputDto> Get(string serviceId)
        {
            var service = (await _serviceRepository.GetPagedListAsync(
                v => v.ServiceId == serviceId, pageSize: 1, include: r => r.Include(s => s.Unit)))
                .Items.FirstOrDefault();

            if (service != null)
                return Mapper.Map<BillableServiceInputDto>(service);

            return null;
        }

        public async Task<IActionResult> Add(ServiceInputDto service)
        {
            if (string.IsNullOrEmpty(service.UnitName))
                return new BadRequestObjectResult("UnitName is required!");

            var unit = (await _unitRepository.GetPagedListAsync(u => u.Name == service.UnitName, pageSize: 1))
                .Items.FirstOrDefault();

            if (unit == null)
                return new BadRequestObjectResult("Unit does not exist!");

            //do not treat unit as insert
            _uow.DbContext.Entry(unit).State = EntityState.Unchanged;

            var input = Mapper.Map<Service>(service);
            input.Unit = unit;

            //save changes
            await _serviceRepository.InsertAsync(input);
            await _uow.SaveChangesAsync();

            return new OkResult();
        }
    }
}
