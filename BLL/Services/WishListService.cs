using BLL.Interfaces;
using DAL.Context;
using DAL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class WishListService : IWishList
    {
        private readonly UserContext user;
        public WishListService(UserContext user)
        {
            this.user = user;
        }
        public async Task<WishList> AddToWishList(int bookId, int userId)
        {
            var wish = user.WhishLists.FirstOrDefault(x => x.BookId == bookId && x.UserId == userId);
            if(wish != null)
            {
                throw new Exception("Book already added to wishlist");
            }
           var wishListItem = new DAL.Models.WishList
           {
               BookId = bookId,
               UserId = userId
           };
            await user.WhishLists.AddAsync(wishListItem);
            await user.SaveChangesAsync();
            return wishListItem;

        }

        public async Task<List<GetAllWishList>> GetWishList(int userId)
        {
            var wishList = await user.WhishLists.Where(x => x.UserId == userId).ToListAsync();
            if (wishList == null)
                throw new Exception("WishList is Empty");
            var res = new List<GetAllWishList>();
            var books = await user.Books.ToListAsync();
            foreach (var item in wishList)
            {
                var wish = new GetAllWishList
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    bookId = item.BookId,
                    BookName = books.Where(x => x.Id == item.BookId).Select(x => x.BookName).FirstOrDefault(), 
                    Author = books.Where(x => x.Id == item.BookId).Select(x => x.Author).FirstOrDefault(), // Added Author mapping
                    Description = books.Where(x => x.Id == item.BookId).Select(x => x.Description).FirstOrDefault(), // Added Description mapping
                    Price = books.Where(x => x.Id == item.BookId).Select(x => x.Price).FirstOrDefault() // Added Price mapping
                };
                res.Add(wish); // Add the constructed wish to the result list
            }
            return res;
        }

        public async Task<WishList> RemoveFromWishList(int bookId, int userId)
        {
            var wish=await user.WhishLists.FirstOrDefaultAsync(x=>x.BookId==bookId && x.UserId == userId);
            if (wish == null)
                throw new Exception("no book found in Wishlist");
            user.WhishLists.Remove(wish);
            await user.SaveChangesAsync();
            return wish;
        }
    }
}
