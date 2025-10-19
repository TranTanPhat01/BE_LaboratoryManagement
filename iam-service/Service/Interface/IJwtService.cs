using iam_service.Modals;


namespace IamService.Services.Interface
{
    public interface IJwtService
    {
        string GenerateToken(Account user, List<string> roles);
    }
}