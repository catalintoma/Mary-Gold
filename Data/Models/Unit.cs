using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marigold
{
    public class Unit
    {
        public string UnitId { get; set; }

        [Required]
        public string Name { get; set; }

        public static string UnitlessKey { get { return "Unitless"; } }

        [NotMapped]
        public bool Unitless { get { return Name == UnitlessKey; } }

        //Simple builder
        public static Unit Named(string name)
        {
            return new Unit
            {
                Name = name
            };
        }
    }
}