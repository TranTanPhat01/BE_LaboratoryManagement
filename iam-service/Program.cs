using Consul;
using iam_service.Data;
using iam_service.GrpcServices;
using iam_service.Repositories.Implement;
using iam_service.Repositories.Interface;
using iam_service.Service.Implement;
using iam_service.Service.Interface;
using IamService.Services.Implement;
using IamService.Services.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Server.Kestrel.Core; // Cần thiết cho HttpProtocols

var builder = WebApplication.CreateBuilder(args);

// ============ Cấu hình Kestrel (Bắt buộc cho lỗi HTTP/2) ============
// Ghi đè cấu hình mặc định để cho phép cả HTTP/1.1 (API/Postman) và HTTP/2 (gRPC) trên endpoint HTTPS
builder.WebHost.UseKestrel(serverOptions =>
{
    // Cấu hình endpoint HTTPS (Giả sử 5001 là HTTPS port)
    serverOptions.ListenAnyIP(5001, listenOptions =>
    {
        // Rất quan trọng: Cho phép cả hai giao thức để giải quyết lỗi "HTTP/2 only endpoint"
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps();
    });

    // Cấu hình endpoint HTTP (Giả sử 5000 là HTTP port)
    serverOptions.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});
// ======================================================================

// ============ 1. DATABASE ============
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UserManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// ============ 2. DI (Dependency Injection) ============
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();

// ============ 3. gRPC ============
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});


// ============ 4. RabbitMQ (MassTransit) ============
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Đã sửa host từ "rabbitmq" sang "localhost" (nếu chạy độc lập)
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

// ============ 5. Swagger + JWT ============
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "iam-service v1", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ============ 6. JWT ============
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
        };
    });
builder.Services.AddAuthorization();

// ============ 7. Consul Client (PHẢI ĐẶT TRƯỚC Build) ============
var consulConfig = builder.Configuration.GetSection("ConsulConfig");
var consulAddress = consulConfig["Address"];

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(config =>
{
    config.Address = new Uri(consulAddress);
}));

// ============ BUILD ============
var app = builder.Build();

// ============ 8. Middleware ============
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
    //db.Database.Migrate();
}   

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ============ 9. gRPC & Controllers ============
app.MapGrpcService<AuthGrpcService>();
app.MapGet("/", () => "IAM gRPC Service is running");
app.MapControllers();

// ============ 10. CONSUL Registration ============
var consulClient = app.Services.GetRequiredService<IConsulClient>();

var serviceName = consulConfig["ServiceName"];
var serviceId = consulConfig["ServiceId"];
var serviceHost = consulConfig["ServiceHost"];
var servicePort = int.Parse(consulConfig["ServicePort"]);
var healthCheckEndpoint = consulConfig["HealthCheckEndpoint"] ?? "/health";

app.MapGet(healthCheckEndpoint, () => Results.Ok("Healthy"));

var registration = new AgentServiceRegistration()
{
    ID = serviceId,
    Name = serviceName,
    Address = serviceHost,
    Port = servicePort,
    Check = new AgentServiceCheck()
    {
        HTTP = $"http://{serviceHost}:{servicePort}{healthCheckEndpoint}",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
    }
};

//await consulClient.Agent.ServiceRegister(registration);

// Gỡ đăng ký khi dừng app
app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(serviceId).Wait();
});

app.Run();