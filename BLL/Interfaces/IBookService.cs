using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookService
    {
        Task<Book> AddBook(BookDTO book);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(int id);
        Task<List<Book>> GetAllBooks(int pageNo,int PageSize);
        Task<List<BookDTO>> GetBooksByNameOrAuthor(string search);
        Task<List<Book>> GetAllBooksInDesc();
    }
}
