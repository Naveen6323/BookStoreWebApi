using BLL.Interfaces;
using DAL.Context;
using DAL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CusttomerDetailService : ICustomerDetailService
    {
        private readonly UserContext user;
        public CusttomerDetailService(UserContext user)
        {
            this.user = user;
        }
        public async Task<CustomerDetail> AddCustomerDetail(CustomerDetailDTO customerDetail, int userId)
        {
            var existingCustomer = await user.CustomerDetails.Where(x=>x.UserId==userId && x.AddressType.Equals(customerDetail.AddressType.ToLower())).FirstOrDefaultAsync();
            
            if (existingCustomer != null)
            {
                
                // Update existing customer details
                existingCustomer.CustomerFullName = customerDetail.CustomerFullName;
                existingCustomer.CustomerAddress = customerDetail.CustomerAddress;
                existingCustomer.CustomerCity = customerDetail.CustomerCity;
                existingCustomer.CustomerState = customerDetail.CustomerState;
                existingCustomer.CustomerPhone = customerDetail.CustomerPhone;
                await user.SaveChangesAsync();
                return existingCustomer;
            }
            else
            {
                // Add new customer details
                var detail = new CustomerDetail
                {
                    CustomerFullName = customerDetail.CustomerFullName,
                    CustomerAddress = customerDetail.CustomerAddress,
                    CustomerCity = customerDetail.CustomerCity,
                    CustomerState = customerDetail.CustomerState,
                    CustomerPhone = customerDetail.CustomerPhone,
                    UserId = userId,
                    AddressType=customerDetail.AddressType.ToLower()
                };
                await user.CustomerDetails.AddAsync(detail);
                await user.SaveChangesAsync();
                return detail;
                
            }
        }



        public async Task<List<CustomerDetail>> GetAllCustomerDetails(int userId)
        {
            var customerDetails = await user.CustomerDetails.Where(c => c.UserId == userId).ToListAsync();
            if (customerDetails.Count==0)
            {
                throw new Exception("No customer details found");
            }
            return customerDetails;
        }
    }
}
