using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserRegistrationDTO> RegisterUser(UserRegistrationDTO user);
        Task<string> LoginUser(Login login);

        Task<List<UserRegistrationDTO>> GetAllUsers(string role);
        string GeneratePasswordResetToken(string email);
        Task SendResetLink(string email);
        Task ResetPassword(string token, string newPassword);
    }
}
