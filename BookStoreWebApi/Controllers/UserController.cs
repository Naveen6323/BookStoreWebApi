using BLL.Interfaces;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService authService;
        public UserController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegistrationDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is null");
                } 
                var data=await authService.RegisterUser(user);
                return Ok(new {isSuccess=true,message="user registered succeccfully",data=data});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(Login login)
        {
            try
            {
                if (login == null)
                {
                    return BadRequest("Login data is null");
                }
                var data = await authService.LoginUser(login);
                return Ok(new { isSuccess = true, message = "user logged in succeccfully", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        }
}
