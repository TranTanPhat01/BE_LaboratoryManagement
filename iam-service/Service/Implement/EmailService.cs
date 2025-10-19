using iam_service.Service.Interface;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string AppPassword { get; set; }
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        // === BẢO VỆ 1: NGĂN LỖI ArgumentNullException (Lỗi trước đó của bạn) ===
        // Nếu địa chỉ nhận bị null hoặc rỗng, không thể gửi email và phải dừng lại.
        if (string.IsNullOrEmpty(toEmail))
        {
            // Tùy chọn: Ghi log ở đây để thông báo lỗi logic ở service gọi
            // Ví dụ: throw new ArgumentNullException(nameof(toEmail), "Địa chỉ người nhận không được để trống.");
            return; // Dừng hàm để ngăn crash ở dòng mail.To.Add(toEmail)
        }

        // === BẢO VỆ 2: Đảm bảo địa chỉ gửi hợp lệ ===
        // Nếu địa chỉ gửi không có, không thể khởi tạo MailMessage, nên dừng lại.
        if (string.IsNullOrEmpty(_settings.SenderEmail))
        {
            // Tùy chọn: Ghi log lỗi cấu hình
            return;
        }

        var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderEmail, _settings.AppPassword),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            // Dòng này cần _settings.SenderEmail phải hợp lệ
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        // Dòng này được bảo vệ bởi kiểm tra 'toEmail' ở đầu hàm
        mail.To.Add(toEmail);

        await client.SendMailAsync(mail);
    }
}