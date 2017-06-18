using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marigold
{
    public class Service
    {
        public string ServiceId { get; set; }

        [Required]
        public string Name { get; set; }

        public int UnitPrice { get; set; }

        [Required]
        public Unit Unit { get; set; }

        public bool Extra { get; set; }

        [NotMapped]
        public bool Unitless { get { return Unit.Unitless; } }

        [NotMapped]
        public bool ExtraCustomPrice { get { return Extra && Unit.Unitless && UnitPrice == 1; } }
    }
}