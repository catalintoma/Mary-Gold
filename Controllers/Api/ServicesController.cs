using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Marigold
{


    [Route("api/v1/[controller]")]
    public class ServiceController : Controller
    {
        private readonly IServicesBll _bll;
        public ServiceController(IServicesBll bll)
        {
            _bll = bll;
        }

        [HttpGet]
        public async Task<IEnumerable<BillableServiceInputDto>> Get()
        {
            return await _bll.AllServices();
        }

        [HttpGet("{id}")]
        public async Task<BillableServiceInputDto> Get(string id)
        {
            return await _bll.Get(id);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(ServiceInputDto input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return new BadRequestObjectResult(errors);
            }
            return await _bll.Add(input);
        }
    }
}
