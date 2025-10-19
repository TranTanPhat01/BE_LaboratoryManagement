using Grpc.Net.Client;
using GrpcAuth;

namespace Service.GrpcService
{
    public class IamGrpcService
    {
        private readonly AuthService.AuthServiceClient _client;

        public IamGrpcService(IConfiguration config)
        {
            // Lấy địa chỉ IAM từ cấu hình
            var iamUrl = config["GrpcSettings:IamServiceUrl"] ?? "https://localhost:5001";
            var channel = GrpcChannel.ForAddress(iamUrl);
            _client = new AuthService.AuthServiceClient(channel);
        }

        public async Task<string> QuickRegisterAsync(string email)
        {
            var request = new QuickRegisterRequest { Email = email };
            var response = await _client.QuickRegisterAsync(request);
            return response.Message;
        }
    }
}
