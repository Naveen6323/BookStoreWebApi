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
    public class cartResponseDTO
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public int bookId { get; set; }
        public string BookName { get;  set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
    }
}
