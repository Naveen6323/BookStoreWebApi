using BLL.Interfaces;
using DAL.Context;
using DAL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserContext user;
        public AuthService(UserContext user) {
            this.user = user;
        }
        public async Task<Login> LoginUser(Login login)
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
            return login;
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
                    Email =newuser.Email,
                    PhoneNumber= newuser.PhoneNumber,
                };
                await user.Users.AddAsync(newUser);
                await user.SaveChangesAsync();
                return newuser;
            }
        }

        public async Task<AdminRegistrationDTO> RegisterAdmin(AdminRegistrationDTO admin)
        {
            var existingUser = user.Admins.Where(x=>x.Email == admin.Email).FirstOrDefaultAsync();
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
                };
                await user.Admins.AddAsync(newUser);
                await user.SaveChangesAsync();
                return admin;
            }

        }

        
    }
}
