using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICustomerDetailService
    {
        Task<CustomerDetail> AddCustomerDetail(CustomerDetailDTO customer, int userId);
        //Task<CustomerDetail> GetCustomerDetail(int cID , int userId);
        Task<List<CustomerDetail>> GetAllCustomerDetails(int userId);
    }
}
