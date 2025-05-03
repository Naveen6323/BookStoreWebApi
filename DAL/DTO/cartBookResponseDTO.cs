using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class cartBookResponseDTO
    {
        
        public string BookName { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
