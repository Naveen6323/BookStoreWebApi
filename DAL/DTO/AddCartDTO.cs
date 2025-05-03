using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class AddCartDTO
    {

        [Required(ErrorMessage = "bookId is required")]
        [ForeignKey("Books")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [DefaultValue(0)]
        public int Quantity { get; set; }

    }
}
