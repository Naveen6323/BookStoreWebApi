﻿using BLL.Interfaces;
using BLL.Services.AuthService;
using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class user : ControllerBase
    {
        private readonly IUserService userService;
        public user(IUserService userService)
        {
            this.userService = userService;
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
                var data = await userService.RegisterUser(user);
                return Ok(new { isSuccess = true, message = "user registered succeccfully", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message, data = string.Empty });
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
                var data = await userService.LoginUser(login);
                return Ok(new { isSuccess = true, message = "user logged in succeccfully", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message =ex.Message, data = string.Empty });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
               
                var data = await userService.GetAllUsers();
                return Ok(new { isSuccess = true, message = "all users", data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message =ex.Message, data = string.Empty });
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string newPassword)
        {
            try
            {
                await userService.ResetPassword(token, newPassword);
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
                await userService.SendResetLink(email);
                return Ok(new { IsSuccess = true, message = "reset link sent", data = email });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, data = ex.Message });
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh( TokenResponse tokenModel)
        {
            try
            {
                var result = await userService.Refresh(tokenModel);
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
