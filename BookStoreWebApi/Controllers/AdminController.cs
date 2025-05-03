using BLL.Interfaces;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IConfiguration _config;
        public AdminController(IAdminService adminService,IConfiguration config, IRefreshToken refreshToken)
        {
            this.adminService = adminService;
            _config = config;
        }
        [HttpPost("admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDTO admin)
        {
            try
            {
                if (admin == null)
                {
                    return BadRequest("Admin data is null");
                }
                var data = await adminService.RegisterAdmin(admin);
                return Ok(new { isSuccess = true, message = "admin registered succeccfully", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin(Login login)
        {
            try
            {
                if (login == null)
                {
                    return BadRequest("Login data is null");
                }
                var data = await adminService.LoginAdmin(login);
                return Ok(new { isSuccess = true, message = "admin logged in succeccfully", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role);
                if (roleClaim == null || string.IsNullOrEmpty(roleClaim.Value))
                {
                    return BadRequest("role is ivalid or null");
                }

                var role = roleClaim.Value;
                var data = await adminService.GetAllAdmins(role);
                return Ok(new { isSuccess = true, message = "all users", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string newPassword)
        {
            try
            {
                await adminService.ResetPassword(token, newPassword);
                return Ok(new { IsSuccess = true, message = "pass reset successsfull", data = newPassword });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = "pass reset unsuccessfull, invalid token" });
            }
        }
        [HttpPost("forgot-pass")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                await adminService.SendResetLink(email);
                return Ok(new { IsSuccess = true, message="reset link sent",data=email});
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false,message=ex.Message, data = string.Empty });
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenResponse tokenModel)
        {
            try
            {
                var result = await adminService.Refresh(tokenModel);
                return Ok(new { IsSuccess = true, message = "token refreshed", data = result });
            }
            catch


            (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, message = ex.Message, data = string.Empty });
            }
        }


    }
}
