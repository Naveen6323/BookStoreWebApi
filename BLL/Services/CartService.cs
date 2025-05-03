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
    public class CartService : ICartService
    {
        private readonly UserContext user;
        public CartService(UserContext user)
        {
            this.user = user;
        }
        public async Task<cartResponseDTO> AddToCart(AddCartDTO cart, int userId)
        {
            var book = await user.Books.FindAsync(cart.BookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            var cartItem = await user.Carts.Where(x => x.bookId == cart.BookId && x.UserId == userId).FirstOrDefaultAsync();
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    bookId = cart.BookId,
                    UserId = userId,
                    Quantity = cart.Quantity,
                    price = await user.Books.Where(x => x.Id == cart.BookId).Select(x => x.Price).FirstOrDefaultAsync()
                };
                await user.Carts.AddAsync(cartItem);
                await user.SaveChangesAsync();
            }
            else
            {
                cartItem.Quantity += cart.Quantity;
                user.Carts.Update(cartItem);
                await user.SaveChangesAsync();
            }

            return new cartResponseDTO
            {
                bookId = cartItem.bookId,
                UserId = cartItem.UserId,
                Quantity = cartItem.Quantity,
                Price = cartItem.price,
                TotalPrice = cartItem.Quantity * cartItem.price,
                book = new cartBookResponseDTO
                {
                    BookName = book.BookName,
                    Author = book.Author,
                    Description = book.Description
                }
            };
            }

        public async Task<string> UpdateCart(int cartId, int quantity, int userId)
        {
            var cartItem = user.Carts.Where(x => x.Id == cartId && x.UserId == userId).FirstOrDefault();
            if (cartItem == null)
            {
                throw new Exception("Cart item not found");
            }
            if (cartItem.Quantity > quantity)
            {
                cartItem.Quantity -= quantity;
                user.Carts.Update(cartItem);
                await user.SaveChangesAsync();
                return "Quantity updated";
            }
            else
            {
                user.Carts.Remove(cartItem);
                await user.SaveChangesAsync();
                return "Cart item removed";
            }
        }
        public async Task<AllCartItemsDTO> GetAllCartItems(int userid)
        {
            var cartItems = await user.Carts.Where(x => x.UserId == userid).ToListAsync();
            if (cartItems == null)
            {
                throw new Exception("Cart is empty");
            }
            var total = 0;
            
            List<cartResponseDTO> cartResponseDTOs = new List<cartResponseDTO>();
            foreach (var item in cartItems) {
                total += item.Quantity * item.price;
                var book = await user.Books.FindAsync(item.bookId);
                if (book == null)
                {
                    throw new Exception("Book not found");
                }
                cartResponseDTOs.Add(new cartResponseDTO
                {
                    bookId = item.bookId,
                    Id=item.Id,
                    UserId = item.UserId,
                    Quantity = item.Quantity,
                    Price = item.price,
                    TotalPrice = item.Quantity * item.price,
                    book = new cartBookResponseDTO
                    {
                        BookName = book.BookName,
                        Author = book.Author,
                        Description = book.Description
                    }
                });
            }

            return new AllCartItemsDTO
            {
                GrandTotal = total,
                CartItems = cartResponseDTOs
            };

        }
            
    }
}
