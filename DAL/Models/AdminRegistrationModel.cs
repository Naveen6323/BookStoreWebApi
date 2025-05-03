using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AdminRegistrationModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [DefaultValue("")]
        [StringLength(50, ErrorMessage = "UserName cannot be longer than 50 characters")]
        public string UserName { get; set; } = string.Empty;
        [DefaultValue("")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters")]
        public string Password { get; set; } = string.Empty;
        [DefaultValue("")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "role is required")]
        [DefaultValue("")]
        public string Role { get; set; } = string.Empty;
        [DefaultValue("")]
        public string refreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }

   
}
