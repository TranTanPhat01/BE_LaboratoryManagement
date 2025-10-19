using CommonServer;                                   // ✅ types dùng chung (GetByIdRequest, IsoDate, ListRequest, ...)
using Google.Protobuf.WellKnownTypes;                 // ✅ Timestamp.FromDateTime
using Grpc.Core;                                      // ✅ gRPC runtime: ServerCallContext, RpcException, StatusCode
using Models.Dto;
using Patient_service.Models.Dto;
using PatientServer;                                  // ✅ types & service base sinh từ patient.proto (csharp_namespace = "PatientServer")
using Service.Interface_service.Repositories.Interface; // ✅ IPatientService
using System;
using System.Linq;
using System.Threading.Tasks;
using GetByIdRequest = CommonServer.GetByIdRequest;

namespace Service.GrpcService
{
    public class PatientGrpcServer : PatientServer.PatientService.PatientServiceBase
    {
        private readonly IPatientService _svc;

        public PatientGrpcServer(IPatientService svc) => _svc = svc;

        public override async Task<PatientResponse> GetPatient(GetByIdRequest request, ServerCallContext context)
        {
            var p = await _svc.GetByIdAsync(request.Id);
            if (p == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Patient {request.Id} not found"));

            var res = new Patient
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                Fullname = p.Fullname,
                // entity: DateOnly?  → proto: string
                Dob = p.Dob.HasValue ? p.Dob.Value.ToString("yyyy-MM-dd") : string.Empty,
                Gender = p.Gender,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                BloodType = p.BloodType,
                IdentityNumber = p.IdentityNumber
                // ⚠️ Không set MedicalHistory/CreatedBy vì proto hiện không có 2 field này
            };

            res.CreatedAt = ToProtoTimestamp(p.CreatedAt);
            res.UpdatedAt = ToProtoTimestamp(p.UpdatedAt);

            return new PatientResponse { Patient = res };
        }

       
        public override async Task<PatientResponse> CreatePatient(CreatePatientRequest request, ServerCallContext context)
        {
            DateOnly? dob = null;
            if (!string.IsNullOrWhiteSpace(request.Patient.Dob) &&
                DateOnly.TryParse(request.Patient.Dob, out var d))
            {
                dob = d;
            }

            var created = await _svc.AddAsync(new PatientDto
            {
                PatientCode = request.Patient.PatientCode,
                Fullname = request.Patient.Fullname,
                Dob = dob,                                // DTO: DateOnly?
                Gender = request.Patient.Gender,
                Phone = request.Patient.Phone,
                Email = request.Patient.Email,
                Address = request.Patient.Address,
                BloodType = request.Patient.BloodType,
                IdentityNumber = request.Patient.IdentityNumber
                // ⚠️ Không set MedicalHistory/CreatedBy vì PatientDto bạn đang dùng không có
            });

            // Trả full entity nếu service trả về
            if (created != null)
            {
                var res = new Patient
                {
                    Id = created.Id,
                    PatientCode = created.PatientCode,
                    Fullname = created.Fullname,
                    Dob = created.Dob.HasValue ? created.Dob.Value.ToString("yyyy-MM-dd") : string.Empty,
                    Gender = created.Gender,
                    Phone = created.Phone,
                    Email = created.Email,
                    Address = created.Address,
                    BloodType = created.BloodType,
                    IdentityNumber = created.IdentityNumber
                };
                return new PatientResponse { Patient = res };

            }

            // Nếu AddAsync trả null, trả tối thiểu (tuỳ business của bạn)
            return new PatientResponse { Patient = new Patient { Id = request.Patient.Id } };
        }
        // -------------------- UpdatePatient --------------------
        public override async Task<PatientResponse> UpdatePatient(UpdatePatientRequest request, ServerCallContext context)
        {
            // proto: dob (string) -> DTO: DateOnly?
            DateOnly? dob = null;
            if (!string.IsNullOrWhiteSpace(request.Patient.Dob) &&
                DateOnly.TryParse(request.Patient.Dob, out var d))
            {
                dob = d;
            }

            // IPatientService.UpdateAsync(PatientUpdate) theo code của bạn
            var update = new PatientUpdate
            {
                Id = request.Patient.Id,
                Fullname = request.Patient.Fullname,
                Dob = dob,
                Gender = request.Patient.Gender,
                Phone = request.Patient.Phone,
                Email = request.Patient.Email,
                Address = request.Patient.Address,
                BloodType = request.Patient.BloodType,
                IdentityNumber = request.Patient.IdentityNumber
            };

            await _svc.UpdateAsync(update);

            // lấy lại bản mới nhất để trả
            var p = await _svc.GetByIdAsync(request.Patient.Id);
            return new PatientResponse { Patient = p != null ? Map(p) : new Patient { Id = request.Patient.Id } };
        }
        // -------------------- ListPatients --------------------
        public override async Task<ListPatientsResponse> ListPatients(PatientServer.ListPatientsRequest request, ServerCallContext context)
        {
            var items = string.IsNullOrWhiteSpace(request.Query)
                ? await _svc.GetAllAsync()
                : await _svc.SearchAsync(request.Query);

            var list = items.ToList();
            var page = request.Page > 0 ? request.Page : 1;
            var size = request.PageSize > 0 ? request.PageSize : 20;

            var slice = list
                .Skip((page - 1) * size)
                .Take(size)
                .Select(Map)
                .ToList();

            var resp = new ListPatientsResponse { Total = list.Count };
            resp.Patients.AddRange(slice);
            return resp;
        }

        // -------------------- Helpers --------------------
        private static Patient Map(Patient_service.Models.Patient p)
        {
            var res = new Patient
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                Fullname = p.Fullname,
                // entity (DateOnly?) -> proto (string)
                Dob = p.Dob.HasValue ? p.Dob.Value.ToString("yyyy-MM-dd") : string.Empty,
                Gender = p.Gender,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                BloodType = p.BloodType,
                IdentityNumber = p.IdentityNumber
            };

            // CreatedAt: DateTime (non-null)
            res.CreatedAt = ToProtoTimestamp(p.CreatedAt);
            // UpdatedAt: DateTime? (nullable)
            res.UpdatedAt = ToProtoTimestamp(p.UpdatedAt);

            return res;
        }
        private static Timestamp ToProtoTimestamp(DateTime dt)
        {
            // nếu Kind chưa set, coi như UTC để tránh lỗi
            if (dt.Kind == DateTimeKind.Unspecified)
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return Timestamp.FromDateTime(dt.ToUniversalTime());
        }

        private static Timestamp? ToProtoTimestamp(DateTime? dt)
        {
            if (!dt.HasValue) return null;
            var v = dt.Value;
            if (v.Kind == DateTimeKind.Unspecified)
                v = DateTime.SpecifyKind(v, DateTimeKind.Utc);
            return Timestamp.FromDateTime(v.ToUniversalTime());
        }
    }
}
