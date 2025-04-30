using BLL.Interfaces;
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
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.user
{
    public class UserService:IUserService
    {
        private readonly IConfiguration _config;
        private readonly UserContext user;
        private readonly IEmailService _emailService;
        public UserService(UserContext user, IConfiguration config,IEmailService email)
        {
            this.user = user;
            _config = config;
            _emailService = email;
        }
        public async Task<string> LoginUser(Login login)
        {
            var user = await this.user.Users.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("user not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                throw new Exception("password is incorrect");
            }
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToLower())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserRegistrationDTO> RegisterUser(UserRegistrationDTO newuser)
        {
            var existingUser = await user.Users.Where(x => x.Email == newuser.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("email already exist");
            }
            else
            {
                newuser.Password = BCrypt.Net.BCrypt.HashPassword(newuser.Password);
                var newUser = new UserRegistrationModel
                {
                    UserName = newuser.UserName,
                    Password = newuser.Password,
                    Email = newuser.Email,
                    PhoneNumber = newuser.PhoneNumber,
                    Role = newuser.Role,
                };
                await user.Users.AddAsync(newUser);
                await user.SaveChangesAsync();
                return newuser;
            }
        }



        public async Task<List<UserRegistrationDTO>> GetAllUsers(string role)
        {

            return await user.Users.Select(x => new UserRegistrationDTO
            {
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();
        }
        public string GeneratePasswordResetToken(string email)
        {
            var admin = user.Users.FirstOrDefault(x => x.Email == email);
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

        public async Task SendResetLink(string email)
        {
            var token = GeneratePasswordResetToken(email);
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

            var resetuser = await user.Users.FindAsync(int.Parse(userId));
            if (resetuser == null)
                throw new Exception("User not found.");

            resetuser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await user.SaveChangesAsync();

        }
    }
}
