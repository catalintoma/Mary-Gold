using System.ComponentModel.DataAnnotations;

namespace Marigold
{
    public class ServiceInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int? UnitPrice { get; set; }
        [Required]
        public string UnitName { get; set; }
        [Required]
        public bool? Extra { get; set; }
    }
}