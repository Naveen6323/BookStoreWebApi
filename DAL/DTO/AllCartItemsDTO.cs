using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class AllCartItemsDTO
    {
        public int GrandTotal { get; set; }
        public List<cartResponseDTO> CartItems { get; set; }
    }
}
