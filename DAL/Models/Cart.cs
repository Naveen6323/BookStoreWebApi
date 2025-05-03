using DAL.DTO;
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
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "bookId is required")]
        [ForeignKey("Books")]
        public int bookId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [DefaultValue(0)]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [DefaultValue(0)]
        public int price { get; set; }

    }
}
