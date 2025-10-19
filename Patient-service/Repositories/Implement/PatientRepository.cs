using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Patient_service.Models;
using Patient_service.Models.Dto;
using Patient_service.Repositories.Interface;

namespace Patient_service.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientServiceContext _context;

        public PatientRepository(PatientServiceContext context)
        {
            _context = context;
        }

        // 📋 Lấy toàn bộ Patients (Không thay đổi)
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .ToListAsync();
        }

        // 🔍 Lấy theo ID (Không thay đổi)
        public async Task<Patient?> GetByIdAsync(string id)
        {
            return await _context.Patients
                //.Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // 🔎 Lấy theo Code (Không thay đổi)
        public async Task<Patient?> GetByCodeAsync(string code)
        {
            return await _context.Patients
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.PatientCode == code);
        }

        // ➕ Thêm mới (Thêm kiểm tra trùng lặp)
        public async Task<Patient> AddAsync(PatientDto patientDto)
        {
            // **[CHECK 1]** Kiểm tra trùng lặp PatientCode
            if (!string.IsNullOrEmpty(patientDto.PatientCode) && await _context.Patients.AnyAsync(p => p.PatientCode == patientDto.PatientCode))
            {
                throw new InvalidOperationException($"PatientCode '{patientDto.PatientCode}' already exists.");
            }

            // **[CHECK 2]** Kiểm tra trùng lặp Phone
            if (!string.IsNullOrEmpty(patientDto.Phone) && await _context.Patients.AnyAsync(p => p.Phone == patientDto.Phone))
            {
                throw new InvalidOperationException($"Phone number '{patientDto.Phone}' already exists.");
            }

            // **[CHECK 3]** Kiểm tra trùng lặp IdentityNumber (nếu có)
            if (!string.IsNullOrEmpty(patientDto.IdentityNumber) && await _context.Patients.AnyAsync(p => p.IdentityNumber == patientDto.IdentityNumber))
            {
                throw new InvalidOperationException($"Identity number '{patientDto.IdentityNumber}' already exists.");
            }

            var patient = new Patient
            {
                Id = "PAT-" + Guid.NewGuid().ToString("N").Substring(0, 10),
                PatientCode = patientDto.PatientCode,
                Fullname = patientDto.Fullname,
                Dob = patientDto.Dob,
                Gender = patientDto.Gender,
                Phone = patientDto.Phone,
                Email = patientDto.Email,
                Address = patientDto.Address,
                BloodType = patientDto.BloodType,
                IdentityNumber = patientDto.IdentityNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Medical_History = patientDto.Medical_History
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        // ➕ Thêm mới (Dành cho tự đăng ký) (Thêm kiểm tra trùng lặp)
        public async Task CreateAccountByPatient(PatientDto patientDto)
        {
            // **[CHECK 1]** Kiểm tra trùng lặp Phone (thường là unique key cho đăng ký)
            if (!string.IsNullOrEmpty(patientDto.Phone) && await _context.Patients.AnyAsync(p => p.Phone == patientDto.Phone))
            {
                throw new InvalidOperationException($"Phone number '{patientDto.Phone}' already exists.");
            }

            // **[CHECK 2]** Kiểm tra trùng lặp Email (nếu là unique key)
            if (!string.IsNullOrEmpty(patientDto.Email) && await _context.Patients.AnyAsync(p => p.Email == patientDto.Email))
            {
                throw new InvalidOperationException($"Email '{patientDto.Email}' already exists.");
            }

            // **[CHECK 3]** Kiểm tra trùng lặp IdentityNumber (nếu có)
            if (!string.IsNullOrEmpty(patientDto.IdentityNumber) && await _context.Patients.AnyAsync(p => p.IdentityNumber == patientDto.IdentityNumber))
            {
                throw new InvalidOperationException($"Identity number '{patientDto.IdentityNumber}' already exists.");
            }

            var patient = new Patient
            {
                Id = "PAT-" + Guid.NewGuid().ToString("N").Substring(0, 5),
                PatientCode = patientDto.PatientCode, // Thường không có code khi tự đăng ký, nên có thể tự sinh ở service/controller
                Fullname = patientDto.Fullname,
                Dob = patientDto.Dob,
                Gender = patientDto.Gender,
                Phone = patientDto.Phone,
                Email = patientDto.Email,
                Address = patientDto.Address,
                BloodType = patientDto.BloodType,
                IdentityNumber = patientDto.IdentityNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Medical_History = patientDto.Medical_History
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(PatientUpdate patientDto)
        {
            // Cảnh báo: Phương thức này giả định rằng patientDto phải có trường Id
            var id = patientDto.Id; // Lấy ID từ DTO để tìm kiếm

            // [CHECK 1] Tìm kiếm bệnh nhân hiện có (ENTITY)
            // KHÔNG dùng AsNoTracking() ở đây để EF có thể theo dõi đối tượng và cập nhật
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPatient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found.");
            }

            // [CHECK 2] Kiểm tra trùng lặp PatientCode với người khác
            if (!string.IsNullOrEmpty(patientDto.PatientCode) &&
                await _context.Patients.AnyAsync(p => p.PatientCode == patientDto.PatientCode && p.Id != id))
            {
                throw new InvalidOperationException($"PatientCode '{patientDto.PatientCode}' already exists for another patient.");
            }

            // [CHECK 3] Kiểm tra trùng lặp Phone với người khác
            if (!string.IsNullOrEmpty(patientDto.Phone) &&
                await _context.Patients.AnyAsync(p => p.Phone == patientDto.Phone && p.Id != id))
            {
                throw new InvalidOperationException($"Phone number '{patientDto.Phone}' already exists for another patient.");
            }

            // [CHECK 4] Kiểm tra trùng lặp IdentityNumber với người khác
            if (!string.IsNullOrEmpty(patientDto.IdentityNumber) &&
                await _context.Patients.AnyAsync(p => p.IdentityNumber == patientDto.IdentityNumber && p.Id != id))
            {
                throw new InvalidOperationException($"Identity number '{patientDto.IdentityNumber}' already exists for another patient.");
            }

            // ------------------------------------------------------------------
            // BƯỚC SỬA LỖI: Ánh xạ dữ liệu từ DTO sang Entity đang được theo dõi
            // ------------------------------------------------------------------

            // *Ánh xạ thủ công (hoặc dùng AutoMapper)*
            existingPatient.PatientCode = patientDto.PatientCode;
            existingPatient.Fullname = patientDto.Fullname;
            existingPatient.Dob = patientDto.Dob; // Ngày sinh
            existingPatient.Gender = patientDto.Gender;
            existingPatient.Phone = patientDto.Phone;
            existingPatient.Email = patientDto.Email;
            existingPatient.Address = patientDto.Address;
            existingPatient.BloodType = patientDto.BloodType;
            existingPatient.IdentityNumber = patientDto.IdentityNumber;
            existingPatient.Medical_History = patientDto.Medical_History; // Lịch sử bệnh lý
            existingPatient.Createby = patientDto.Createby;
            // ------------------------------------------------------------------

            // Cập nhật thời gian: Áp dụng cho Entity chứ không phải DTO
            existingPatient.UpdatedAt = DateTime.UtcNow;

            _context.Patients.Update(existingPatient);
            await _context.SaveChangesAsync();
        }

        // ❌ Xóa (Thêm kiểm tra tồn tại)
        public async Task DeleteAsync(string id)
        {
            var patient = await _context.Patients.FindAsync(id);

            // **[CHECK 1]** Kiểm tra bệnh nhân có tồn tại không
            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found.");
            }

            // Note: EF Core sẽ tự động xử lý các MedicalRecords liên quan nếu bạn đã cấu hình Cascade Delete.
            // Nếu không, bạn cần thêm logic xóa/hủy kích hoạt MedicalRecords ở đây.

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Patient>> SearchAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new List<Patient>();

            key = key.Trim().ToLower();

            // Tìm theo nhiều trường cùng lúc
            var query = _context.Patients.AsQueryable();

            query = query.Where(p =>
                (!string.IsNullOrEmpty(p.PatientCode) && p.PatientCode.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.Fullname) && p.Fullname.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.Phone) && p.Phone.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.Email) && p.Email.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.Address) && p.Address.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.BloodType) && p.BloodType.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.IdentityNumber) && p.IdentityNumber.ToLower().Contains(key)) ||
                (!string.IsNullOrEmpty(p.Gender) && p.Gender.ToLower().Contains(key))
            );

            // Có thể mở rộng thêm: tìm theo ngày sinh (DOB)
            if (DateOnly.TryParse(key, out var dob))
            {
                query = query.Union(_context.Patients.Where(p => p.Dob == dob));
            }

            // Tìm kiếm kết quả
            return await query
                .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _context.Patients
               .FirstOrDefaultAsync(p => p.Email == email);
        }
    }
    }
