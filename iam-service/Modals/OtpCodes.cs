namespace iam_service.Modals
{
    public partial class OtpCodes
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!; // Renamed property to 'Code' to avoid conflict with class name  
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int Status { get; set; }
    }
}
