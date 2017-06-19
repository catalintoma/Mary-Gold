using System.ComponentModel.DataAnnotations;

namespace Marigold
{
    public class BillableServiceInputDto
    {
        public BillableServiceInputDto()
        {
            Units = 1;
        }

        [Required]
        public string ServiceId { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool Unitless { get; set; }

        public bool Extra{get;set;}

        public string UnitDescription { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Min. value: 1")]
        public int Units { get; set; }
    }
}