using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;
using GrpcAuth;

using MassTransit;
using Microsoft.EntityFrameworkCore;


using Patient_service;
using Patient_service.MappingProfiles;
using Patient_service.Repositories;
using Patient_service.Repositories.Interface;
using Repositories.Interface;
using Service.GrpcService;
using Service.Implement;
using Service.Interface;
using Service.Interface_service.Repositories.Interface;

// Đã loại bỏ using Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.IdentityModel.Tokens, System.Text

var builder = WebApplication.CreateBuilder(args);

// ---------------------- Configuration ----------------------
var configuration = builder.Configuration;
var env = builder.Environment;

// ---------------------- 1. EF Core (DbContext) ----------------------
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PatientServiceContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// ---------------------- 2. AutoMapper ----------------------
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(PatientMappingProfile).Assembly);
});

// ---------------------- 3. Register Repositories ----------------------
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<ILabTestRepository, LabTestRepository>();
builder.Services.AddScoped<ILabCriterionRepository, LabCriterionRepository>();

// ---------------------- 4. Register Services (business layer) ----------------------
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<ILabTestService, LabTestService>();
builder.Services.AddScoped<ILabCriterionService, LabCriterionService>();

// ---------------------- 5. HttpClientFactory ----------------------
builder.Services.AddHttpClient();

// =========================================================================================
// 6. gRPC Client: AuthService (IAM) - KHẮC PHỤC LỖI DI
// =========================================================================================
builder.Services.AddScoped<IamGrpcService>(); // <-- ĐÃ BỎ GHI CHÚ
builder.Services.AddGrpc();


// =========================================================================================

// =========================================================================================
// 7. CẤU HÌNH BẢO MẬT (Đã xóa các dịch vụ Authorization để test kết nối)
// =========================================================================================
// Ghi chú builder.Services.AddAuthentication(...)
// Ghi chú builder.Services.AddAuthorization() để không đăng ký dịch vụ Authorization
// =========================================================================================

// ---------------------- 8. MassTransit (FIX: Sử dụng In-Memory) ----------------------
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) => // Đã bỏ qua lỗi RabbitMQ
    {
        cfg.ConfigureEndpoints(context);
    });
});

// ---------------------- 9. Controllers + JSON options ----------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// ---------------------- 10. Swagger ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------- 11. Build app ----------------------
var app = builder.Build();

// ---------------------- 12. Middleware pipeline ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapGrpcService<PatientGrpcServer>();
app.MapGrpcService<MedicalRecordGrpcServer>();
app.MapGrpcService<LabGrpcServer>();

// === KHÔNG CÓ UseAuthentication và UseAuthorization ===
// app.UseAuthentication(); 
// app.UseAuthorization(); 

app.MapControllers();

// lightweight health/ping endpoint
app.MapGet("/", () => Results.Ok("✅ Patient Service: Ready for API & gRPC client usage."));

app.Run();
