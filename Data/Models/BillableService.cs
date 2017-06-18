using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marigold
{
    public class BillableService
    {
        public string BillableServiceId { get; set; }

        [Required]
        public Service Service { get; set; }

        public string ServiceId { get; set; }



        public int Units { get; set; }
    }
}