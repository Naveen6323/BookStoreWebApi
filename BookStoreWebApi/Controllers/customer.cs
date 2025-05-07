using BLL.Interfaces;
using DAL.DTO;
using DAL.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class customer : ControllerBase
    {
        private readonly ICustomerDetailService cust;
        public customer(ICustomerDetailService cust)
        {
            this.cust = cust;
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetailDTO customer)
        {
            try
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null)
                {
                    return BadRequest(new { isSuccess = false, message = "User not found", data = string.Empty });
                }
                int userId = int.Parse(claim.Value);

                var res=await cust.AddCustomerDetail(customer,userId);
                return Ok(new { isSuccess = true, message = "added successfully", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message, data = string.Empty });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerDetail()
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var res = await cust.GetAllCustomerDetails(userId);
                return Ok(new { isSuccess = true, message = "fetched successfully", data = res });
            }
            catch (Exception ex)
            {
                return BadRequest(new { isSuccess = false, message = ex.Message, data = string.Empty });
            }
        }

    }
}
