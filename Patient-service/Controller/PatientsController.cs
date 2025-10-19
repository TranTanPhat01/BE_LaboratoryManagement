using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Patient_service.Models;
using Service.Interface_service.Repositories.Interface;
using Models.Dto;
using System.Linq;
using Patient_service.Models.Dto;
// using Microsoft.AspNetCore.Authorization; // <-- Đã xóa using này

namespace Patient_service.Controllers
{
    [ApiController]
    [Route("api/patients")]
    // [Authorize] // <-- ĐÃ XÓA
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService) 
        {
            _patientService = patientService;
        }

       
        [HttpGet]
       
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var patients = await _patientService.GetAllAsync();
            var total = patients.Count();
            var items = patients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            if (!items.Any())
                return NotFound(new { message = "Không có bệnh nhân nào." });

            return Ok(new
            {
                message = "Lấy danh sách bệnh nhân thành công.",
                page,
                pageSize,
                total,
                data = items
            });
        }


        // Lấy chi tiết bệnh nhân theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(new { message = $"Không tìm thấy bệnh nhân với ID: {id}" });
            return Ok(new { message = "Lấy bệnh nhân thành công.", data = patient });
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmmail(string email)
        {
            var patient = await _patientService.GetByEmailAsync(email);
            if (patient == null)
                return NotFound(new { message = $"Không tìm thấy hồ sơ bệnh nhân với! " });
                return Ok(new { message = "Lấy hồ sơ bệnh nhân thành công.", data = patient });
        }
        // Ví dụ: API kiểm tra thông tin bệnh nhân (bạn đổi lại cho phù hợp)
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                // Trả về thất bại nếu thiếu tham số tìm kiếm
                return BadRequest(new { message = "Thiếu từ khóa tìm kiếm." });
            }

            var patient = await _patientService.SearchAsync(key);
            if (patient == null)
            {
                // Trả về thất bại nếu không tìm thấy
                return NotFound(new { message = $"Không tìm thấy bệnh nhân với từ khóa: {key}" });
            }

            // Trả về thành công nếu tìm thấy
            return Ok(new { message = "Tìm thấy bệnh nhân.", data = patient });
        }


        // Tạo mới bệnh nhân
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientDto patient)
        {
            var newPatient = await _patientService.AddAsync(patient);
            if (newPatient == null)
                return NotFound(new { message = "Tạo bệnh nhân thất bại." });
            return Ok(new { message = "Tạo bệnh nhân thành công.", data = newPatient });
        }

        // Cập nhật bệnh nhân
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PatientUpdate patient)
        {
            if (id != patient.Id)
                return BadRequest(new { message = "Id không khớp." });

            var existing = await _patientService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy bệnh nhân với ID: {id}" });

            await _patientService.UpdateAsync(patient);
            return Ok(new { message = "Cập nhật bệnh nhân thành công." });
        }

        // Xóa bệnh nhân
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _patientService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy bệnh nhân với ID: {id}" });

            await _patientService.DeleteAsync(id);
            return Ok(new { message = "Xóa bệnh nhân thành công." });
        }

        // Đăng ký bệnh nhân mới và gửi email
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PatientDto patient)
        {
            var newPatient = await _patientService.AddAsync(patient);
            if (newPatient == null)
                return NotFound(new { message = "Tạo bệnh nhân thất bại." });

            await _patientService.CreateAccountbyEmailAsync(patient);
            return Ok(new { message = "Tạo bệnh nhân và gửi đăng ký tài khoản thành công.", data = newPatient });
        }
    }
}
