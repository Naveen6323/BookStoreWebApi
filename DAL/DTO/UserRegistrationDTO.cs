using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class UserRegistrationDTO
    {
        
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName cannot be longer than 50 characters")]
        [DefaultValue("")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters")]
        [DefaultValue("")]

        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DefaultValue("")]

        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "PhoneNumber must be 10 digits")]
        [DefaultValue("")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "role is required")]
        [DefaultValue("")]
        public string Role { get; set; } = string.Empty;
    }
}
