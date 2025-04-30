using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAdminService
    {
        Task<string> LoginAdmin(Login login);
        Task<AdminRegistrationDTO> RegisterAdmin(AdminRegistrationDTO admin);
        Task<List<AdminRegistrationDTO>> GetAllAdmins(string role);
        string GeneratePasswordResetToken( string email);
        Task SendResetLink( string email);
        Task ResetPassword(string token, string newPassword);
    }
}
