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
    public class BookService : IBookService
    {
        private readonly UserContext user;
        public BookService(UserContext context)
        {
            this.user = context;
        }
        public async Task<Book> AddBook(BookDTO book)
        {
           
            var newBook = new Book
            {
                BookName = book.BookName,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                Quantity = book.Quantity,
                BookImage = book.BookImage,
                CreatedAtDate = DateTime.UtcNow,
                UpdatedAtDate = DateTime.UtcNow,
                AdminUserId = book.AdminUserId
            };
            await user.Books.AddAsync(newBook);
            await user.SaveChangesAsync();
            return newBook;

        }

        public async Task<Book> DeleteBook(int id)
        {
            
            var book = user.Books.FirstOrDefault(x=>x.Id==id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            user.Books.Remove(book);
            await user.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllBooks(int pageNo,int pageSize)
        {
            var books=await user.Books.Skip((pageNo-1)*pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
            return books;
        }
        public async Task<List<Book>> GetAllBooksInDesc()
        {
            var books = await user.Books
                .OrderByDescending(x => x.DiscountPrice)
                .ToListAsync();
            return books;
        }
        public async Task<List<BookDTO>> GetBooksByNameOrAuthor(string search)
        {
            var books = await user.Books
                .Where(x => x.BookName.Contains(search) || x.Author.Contains(search))
                .Select(x => new BookDTO
                {
                    BookName = x.BookName,
                    Author = x.Author,
                    Description = x.Description,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    Quantity = x.Quantity,
                    BookImage = x.BookImage
                })
                .ToListAsync();
            return books;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            

            var existingBook = await user.Books.FirstOrDefaultAsync(x => x.Id == book.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            // Update properties using Entity Framework's SetPropertyCalls
            await user.Books
                .Where(x => x.Id == book.Id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(x => x.BookName, book.BookName)
                    .SetProperty(x => x.Author, book.Author)
                    .SetProperty(x => x.Description, book.Description)
                    .SetProperty(x => x.Price, book.Price)
                    .SetProperty(x => x.DiscountPrice, book.DiscountPrice)
                    .SetProperty(x => x.Quantity, book.Quantity)
                    .SetProperty(x => x.BookImage, book.BookImage)
                    .SetProperty(x => x.UpdatedAtDate, DateTime.UtcNow)
                );
            await user.SaveChangesAsync();

            return await user.Books.FirstOrDefaultAsync(x => x.Id == book.Id);
        }
    }
}
