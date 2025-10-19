using Grpc.Core;
using GrpcAuth;
using iam_service.Repositories.Interface;
using IamService.Services.Interface;

namespace iam_service.GrpcServices
{
    public class AuthGrpcService : AuthService.AuthServiceBase
    {
        private readonly IAuthService _authService;

        public AuthGrpcService(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<QuickRegisterResponse> QuickRegister(
            QuickRegisterRequest request,
            ServerCallContext context)
        {
            var message = await _authService.RegisterAsync(request.Email);
            return new QuickRegisterResponse { Message = message };
        }
    }
}
