using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class BookDTO
    {
       
        [Required(ErrorMessage = "Description is required")]
        [StringLength(750, ErrorMessage = "Description cannot be longer than 750 characters")]
        [DefaultValue("")]
        public string Description { get; set; }
        [Required(ErrorMessage = "DiscountPrice is required")]
        [DefaultValue(0)]
        public int DiscountPrice { get; set; }
        [DefaultValue("")]
        [StringLength(300, ErrorMessage = "BookImage cannot be longer than 300 characters")]

        public string? BookImage { get; set; }
        [Required(ErrorMessage = "Admin_id is required")]
        [DefaultValue("")]
        public string AdminUserId { get; set; }
        [Required(ErrorMessage = "BookName is required")]
        [StringLength(50, ErrorMessage = "BookName cannot be longer than 50 characters")]
        [DefaultValue("")]

        public string BookName { get; set; } = null!;
        [Required(ErrorMessage = "Author is required")]
        [DefaultValue("")]
        [StringLength(50, ErrorMessage = "Author cannot be longer than 50 characters")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [DefaultValue(0)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [DefaultValue(0)]
        public int Price { get; set; }
        
    }
}
