using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWishList
    {
        Task<WishList> AddToWishList(int bookId, int userId);
        Task<WishList> RemoveFromWishList(int bookId, int userId);
        Task<List<GetAllWishList>> GetWishList(int userId);
    }
}
