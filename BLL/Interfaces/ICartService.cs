using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICartService
    {
        Task<cartResponseDTO> AddToCart(AddCartDTO cart,int userId);
        Task<string> UpdateCart(int cartId, int quantity, int userId);
        Task<AllCartItemsDTO> GetAllCartItems(int userid);
        //Task<bool> UpdateCartItem(int bookId, int userId, int quantity);
        //Task<bool> ClearCart(int userId);
    }
}
