using BLL.Interfaces;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Books : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IConfiguration config;
        public Books(IBookService bookService,IConfiguration config)
        {
            this.bookService = bookService;
            this.config = config;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddBook(BookDTO book)
        {
            try
            {
                //var claim = User.FindFirst(ClaimTypes.Role);
                //if (claim == null)
                //    return BadRequest(new { IsSuccess = false, message = "unauthorized", data = string.Empty });
                //var role = claim.Value;
                var res = await bookService.AddBook(book);
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks(int pageNo,int pageSize)
        {
            try
            {
                var validPageSIze = pageSize > int.Parse(config["Pagination:PageSize"]) ? pageSize: int.Parse(config["Pagination:PageSize"]);

                var books = await bookService.GetAllBooks(pageNo,validPageSIze);
                return Ok(new { IsSuccess = true, message = "successfull", data = books });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpGet("getByNameOrAuthor")]
        public async Task<IActionResult> GetByNameOrAuthor(string search)
        {
            try
            {
                var books = await bookService.GetBooksByNameOrAuthor(search);
                return Ok(new { IsSuccess = true, message = "successfull", data = books });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = true, message = ex.Message, data = string.Empty });
            }

        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updateBook(Book book)
        {
            try
            {
               
                var res = await bookService.UpdateBook(book);
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            try
            {
                var res = await bookService.DeleteBook(bookId);
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpGet("getAllBooksInDesc")]
        public async Task<IActionResult> GetAllBooksInDesc()
        {
            try
            {
                var books = await bookService.GetAllBooksInDesc();
                return Ok(new { IsSuccess = true, message = "successfull", data = books });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
    }
}
