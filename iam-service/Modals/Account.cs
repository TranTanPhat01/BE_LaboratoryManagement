using System.Reflection.Metadata;

namespace iam_service.Modals
{
    public partial class Account
    {
        public string Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
