using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marigold
{
    public interface IServicesBll
    {
        Task<IEnumerable<BillableServiceInputDto>> AllServices();

        Task<BillableServiceInputDto> Get(string serviceId);
        
        Task<IActionResult> Add(ServiceInputDto input);
    }
}