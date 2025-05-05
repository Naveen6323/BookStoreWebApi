using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class wishList : ControllerBase
    {
        private readonly IWishList wish;
        public wishList(IWishList wish)
        {
            this.wish = wish;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await wish.GetWishList(int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToWishList(int bookId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await wish.AddToWishList(bookId, int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int bookId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await wish.RemoveFromWishList(bookId, int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
    }
}
