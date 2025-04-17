using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task<UserRegistrationDTO> RegisterUser(UserRegistrationDTO user);
        Task<Login> LoginUser(Login login);
        Task< AdminRegistrationDTO>RegisterAdmin(AdminRegistrationDTO admin);
    }
}
