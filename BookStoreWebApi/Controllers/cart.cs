using BLL.Interfaces;
using DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class cart : ControllerBase
    {
        private readonly ICartService cartService;
        public cart(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return BadRequest(new { IsSuccess = false, message = "User not found", data = string.Empty });
                }
                
                var res = await cartService.AddToCart(bookId, int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCart(int bookId, int quantity)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return BadRequest(new { IsSuccess = false, message = "User not found", data = string.Empty });
                }
                
                var res = await cartService.UpdateCart(bookId,quantity, int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return BadRequest(new { IsSuccess = false, message = "User not found", data = string.Empty });
                }

                var res = await cartService.GetAllCartItems(int.Parse(userId));
                return Ok(new { IsSuccess = true, message = "successfull", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }

    }
}
