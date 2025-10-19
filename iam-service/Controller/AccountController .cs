using IamService.Services.Interface;
using iam_service.Modals.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace IamService.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Chỉ user có role Admin mới truy cập được
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Lấy danh sách tài khoản có phân trang
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var accounts = await _accountService.GetAllAsync();
            var total = accounts.Count();
            var items = accounts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (!items.Any())
            {
                return NotFound(new { message = "Không có tài khoản nào." });
            }

            return Ok(new
            {
                message = "Lấy danh sách tài khoản thành công.",
                page,
                pageSize,
                total,
                data = items
            });
        }

        // Lấy chi tiết tài khoản theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var account = await _accountService.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { message = $"Không tìm thấy tài khoản với ID: {id}" });
            }

            return Ok(new { message = "Lấy tài khoản thành công.", data = account });
        }

        // Tạo mới tài khoản
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDto accountDto)
        {
            var createdAccount = await _accountService.CreateAsync(accountDto);
            if (createdAccount == null)
            {
                return NotFound(new { message = "Tạo tài khoản thất bại." });
            }
            return Ok(new { message = "Tạo tài khoản thành công.", data = createdAccount });
        }

        // Cập nhật tài khoản
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AccountDto accountDto)
        {
            var updatedAccount = await _accountService.UpdateAsync(id, accountDto);

            if (updatedAccount == null)
            {
                return NotFound(new { message = $"Không tìm thấy tài khoản để cập nhật với ID: {id}" });
            }

            return Ok(new { message = "Cập nhật tài khoản thành công.", data = updatedAccount });
        }

        // Xóa tài khoản
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await _accountService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { message = $"Không tìm thấy tài khoản để xóa với ID: {id}" });
            }

            return Ok(new { message = "Xóa tài khoản thành công." });
        }
    }
}
