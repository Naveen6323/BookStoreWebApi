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
        Task<TokenResponse> LoginAdmin(Login login);
        Task<AdminRegistrationDTO> RegisterAdmin(AdminRegistrationDTO admin);
        Task<List<AdminRegistrationDTO>> GetAllAdmins();
        string GeneratePasswordResetToken( string email);
        Task SendResetLink( string email);
        Task ResetPassword(string token, string newPassword);
        Task<TokenResponse> Refresh(TokenResponse tokenModel);
        
    }
}
