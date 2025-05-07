using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CustomerDetail
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage ="Full name is required")]
        [DefaultValue("")]
        public string CustomerFullName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [DefaultValue("")]

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
        [DefaultValue("")]
        public string AddressType { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
