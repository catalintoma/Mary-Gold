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


        [Range(1,int.MaxValue,ErrorMessage = "Please specify a positive number!")]
        public int Units { get; set; }
    }
}