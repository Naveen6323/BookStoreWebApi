using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class CustomerDetailDTO
    {
        [Required(ErrorMessage = "Full name is required")]
        [DefaultValue("")]
        public string CustomerFullName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DefaultValue("")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "PhoneNumber must be 10 digits")]

        public string CustomerPhone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [DefaultValue("")]

        public string CustomerAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        [DefaultValue("")]

        public string CustomerCity { get; set; }
        [Required(ErrorMessage = "State is required")]
        [DefaultValue("")]

        public string CustomerState { get; set; }
        [Required(ErrorMessage = "Address type is required")]
        [DefaultValue("home")]
        public string AddressType { get; set; }
    }
}
