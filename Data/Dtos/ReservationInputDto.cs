using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marigold
{
    public class ReservationInputDto : IValidatableObject
    {
        public ReservationInputDto()
        {
            CheckinDate = DateTime.Now;
            CheckoutDate = DateTime.Now.AddDays(1);
        }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Customer Phone Number")]
        [RegularExpression(@"^[0-9-]*$", ErrorMessage = "Please enter a valid phone number!")]
        public string CustomerPhoneNo { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [Display(Name = "Checkin date")]
        public DateTime CheckinDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [Display(Name = "Checkout date")]
        public DateTime CheckoutDate { get; set; }

        public List<ServiceInputDto> Services { get; set; }

        public List<SelectListItem> Rooms { get; set; }

        [Display(Name = "Room type")]
        public string SelectedRoomId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckoutDate <= CheckinDate)
            {
                yield return new ValidationResult("Checkout date must be after checkin date!", memberNames: new[] { "CheckoutDate" });
            }
        }
    }
}