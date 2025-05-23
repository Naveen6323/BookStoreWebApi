﻿using BLL.Interfaces;
using BLL.Services.Email;
using DAL.Context;
using DAL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AuthService
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _config;
        private readonly UserContext user;
        private readonly IEmailService _emailService;
        private readonly IRefreshToken refresh;
        public AdminService(UserContext user, IConfiguration config,IEmailService emailService,IRefreshToken refresh)
        {
            this.user = user;
            _config = config;
            _emailService = emailService;
            this.refresh = refresh;
        }
    
    
        public async Task<AdminRegistrationDTO> RegisterAdmin(AdminRegistrationDTO admin)
        {
            var existingUser = await user.Admins.Where(x => x.Email == admin.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("email already exist");
            }
            else
            {
                admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);
                var newUser = new AdminRegistrationModel
                {
                    UserName = admin.UserName,
                    Password = admin.Password,
                    Email = admin.Email,
                    Role = admin.Role,
                };
                await user.Admins.AddAsync(newUser);
                await user.SaveChangesAsync();
                return admin;
            }

        }



        public async Task<TokenResponse> LoginAdmin(Login login)
        {
            var admin = await user.Admins.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            if (admin == null)
            {
                throw new Exception("admin not found");
            }
            else if (!BCrypt.Net.BCrypt.Verify(login.Password, admin.Password))
            {
                throw new Exception("password is incorrect");
            }
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Role, admin.Role.ToLower())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int expireTime =int.Parse(_config["Jwt:Expire"]);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expireTime),
                claims: claims,
                signingCredentials: creds);
            var refreshToken = refresh.GenerateRefreshToken();

            admin.refreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.Now.AddDays(int.Parse(_config["Jwt:RefreshExpire"]));
            await user.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
            };

        }
        
        public async Task<TokenResponse> Refresh( TokenResponse tokenModel)
        {
            var principal = refresh.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal == null)
                throw new Exception("Invalid principle");
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid userid");

            var storedToken = await user.Admins.FirstOrDefaultAsync(x=>x.Id== int.Parse(userId)&& x.refreshToken==tokenModel.RefreshToken);
            if (storedToken == null || storedToken.RefreshTokenExpiry < DateTime.Now)
                throw new Exception("Invalid token");

            var newAccessToken = refresh.GenerateAccessToken(principal.Claims);
            var newRefreshToken = refresh.GenerateRefreshToken();

            // Update refresh token in DB
            storedToken.refreshToken = newRefreshToken;
            storedToken.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await user.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }



        public async Task<List<AdminRegistrationDTO>> GetAllAdmins()
        {
            
            return await user.Admins.Select(x => new AdminRegistrationDTO
            {
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
                Role=x.Role
            }).ToListAsync();
        }
        public string GeneratePasswordResetToken(string email)
        {
            var admin = user.Admins.FirstOrDefault(x => x.Email == email);
            if (admin == null)
            {
                throw new Exception("Admin not found");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Email, admin.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ResetKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SendResetLink( string email)
        {
            var token = GeneratePasswordResetToken( email);
            var resetLink = $"{token}";

            var subject = "Password Reset Request";
            var body = $"<p>Click the link below to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>";

            await _emailService.SendEmailAsync(email, subject, body);
        }
        public async Task ResetPassword(string token, string newPassword)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:ResetKey"]);

            
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Invalid token.");

                var resetuser = await user.Admins.FindAsync(int.Parse(userId));
                if (resetuser == null)
                    throw new Exception ("User not found.");

                resetuser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await user.SaveChangesAsync();

        }
        

    }
}
