using ASC.Web.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ASC.Web.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private IOptions<ApplicationSettings> _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("admin", _settings.Value.SMTPAccount));
            emailMessage.To.Add(new MailboxAddress("user", email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_settings.Value.SMTPServer, _settings.Value.SMTPPort, false);
                await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Triển khai gửi SMS ở đây
            // Bạn có thể sử dụng ISmsSender đã có của mình
            // để thực hiện việc gửi SMS thực tế
            // Ví dụ: _yourSmsSender.SendSmsAsync(number, message); 
            // Nếu bạn không có triển khai cụ thể, bạn có thể 
            // ghi log hoặc trả về Task.CompletedTask

            // Ví dụ ghi log:
            System.Console.WriteLine($"SMS sent to {number}: {message}");

            // Ví dụ trả về Task.CompletedTask
            await Task.CompletedTask;
        }
    }
}