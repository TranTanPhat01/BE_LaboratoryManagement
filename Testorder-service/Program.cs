
using Microsoft.EntityFrameworkCore;
using PatientServer;
using Testorder_service.Repositories.Implement;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Implement;
using Testorder_service.Service.Interface;
using TestOrderService.Data;

namespace Testorder_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cho phép HTTP/2 không mã hoá khi dùng http:// ở môi trường dev
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //builder.Services.AddGrpcClient<PatientServer.PatientService.PatientServiceClient>(o =>
            //{
            //    o.Address = new Uri("https://localhost:5002"); // ← ĐỔI CHỖ NÀY
            //});

            builder.Services.AddAutoMapper(cfg =>
            {
                // có thể add từng profile:
                cfg.AddProfile<Testorder_service.Mapping.Profiles.BloodSamplesProfile>();
                cfg.AddProfile<Testorder_service.Mapping.Profiles.TestResultsProfile>();
                cfg.AddProfile<Testorder_service.Mapping.Profiles.TestParametersProfile>();
                cfg.AddProfile<Testorder_service.Mapping.Profiles.FlaggingConfigsProfile>();
                cfg.AddProfile<Testorder_service.Mapping.Profiles.FlaggingLogsProfile>();
                cfg.AddProfile<Testorder_service.Mapping.Profiles.ResultCommentsProfile>();
            });

            // Services
            builder.Services.AddScoped<IBloodSampleRepository, BloodSampleRepository>();
            builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();
            builder.Services.AddScoped<ITestParameterRepository, TestParameterRepository>();
            builder.Services.AddScoped<IFlaggingConfigRepository, FlaggingConfigRepository>();
            builder.Services.AddScoped<IFlaggingLogRepository, FlaggingLogRepository>();
            builder.Services.AddScoped<IResultCommentRepository, ResultCommentRepository>();

            var conn = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<TestDbContext>(opt =>
                opt.UseNpgsql(conn, o => o.UseNodaTime()));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // === Đăng ký gRPC Client đến Patient-service ===
            builder.Services.AddGrpcClient<PatientService.PatientServiceClient>(o =>
            {
                // Đọc endpoint server từ appsettings.json -> "Grpc:PatientService"
                o.Address = new Uri(builder.Configuration["Grpc:PatientService"]!);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // ping nhẹ
            app.MapGet("/", () => Results.Ok("✅ Testorder-service up. gRPC client ready."));

            app.Run();
        }
    }
}
