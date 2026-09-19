using HSLR.Models.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HSLR.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent, string? replyTo = null)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "localhost";
            var smtpPort = int.TryParse(_configuration["EmailSettings:SmtpPort"], out var port) ? port : 587;
            var senderName = _configuration["EmailSettings:SenderName"] ?? "HSRL UET Lahore";
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "hsrl@uet.edu.pk";
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var useDevLogger = bool.TryParse(_configuration["EmailSettings:UseDevelopmentLogger"], out var dev) && dev;

            if (useDevLogger || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogInformation(
                    "[EmailService: Dev Mode] Email to: {ToEmail} | Subject: {Subject} | ReplyTo: {ReplyTo}\nContent:\n{Content}",
                    toEmail, subject, replyTo, htmlContent);
                return;
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                if (!string.IsNullOrWhiteSpace(replyTo))
                {
                    message.ReplyTo.Add(MailboxAddress.Parse(replyTo));
                }
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlContent
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                // Do not crash the web app on email send failure in development/production
            }
        }

        public async Task SendInquiryNotificationAsync(CollaborationInquiry inquiry, SiteConfig siteConfig)
        {
            var subject = $"[HSRL Inquiry #{inquiry.Id}] New Collaboration Request from {inquiry.FullName} ({inquiry.Organization})";
            var body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #1e293b; max-width: 640px; margin: 0 auto; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden;'>
                    <div style='background-color: #0a2540; color: #ffffff; padding: 24px;'>
                        <h2 style='margin: 0 0 8px 0; font-size: 20px; font-weight: bold;'>New Research / Collaboration Inquiry</h2>
                        <p style='margin: 0; font-size: 14px; opacity: 0.85;'>Hydroclimatic Sensing Research Lab (HSRL) — CEWRE, UET Lahore</p>
                    </div>
                    <div style='padding: 24px;'>
                        <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; width: 35%; border-bottom: 1px solid #f1f5f9;'>Inquiry ID:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>#{inquiry.Id}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Submitted At:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.SubmittedAtUtc:yyyy-MM-dd HH:mm} UTC</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Full Name:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.FullName}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Email:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'><a href='mailto:{inquiry.Email}'>{inquiry.Email}</a></td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Organization:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.Organization}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Affiliation Type:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.AffiliationType}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Collaboration Type:</td>
                                <td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.CollaborationType}</td>
                            </tr>
                            {(string.IsNullOrWhiteSpace(inquiry.Title) ? "" : $"<tr><td style='padding: 8px 0; font-weight: bold; border-bottom: 1px solid #f1f5f9;'>Proposal / Subject:</td><td style='padding: 8px 0; border-bottom: 1px solid #f1f5f9;'>{inquiry.Title}</td></tr>")}
                        </table>
                        <h4 style='margin: 16px 0 8px 0; color: #0a2540;'>Inquiry Description:</h4>
                        <div style='background: #f8fafc; padding: 16px; border-radius: 6px; border: 1px solid #e2e8f0; white-space: pre-wrap;'>{inquiry.Description}</div>
                    </div>
                </div>";

            await SendEmailAsync(siteConfig.ContactEmail, subject, body, replyTo: inquiry.Email);
        }

        public async Task SendInquiryConfirmationAsync(CollaborationInquiry inquiry, SiteConfig siteConfig)
        {
            var subject = $"Confirmation: Research Collaboration Inquiry Received — HSRL UET Lahore (#{inquiry.Id})";
            var body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #1e293b; max-width: 640px; margin: 0 auto; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden;'>
                    <div style='background-color: #0a2540; color: #ffffff; padding: 24px;'>
                        <h2 style='margin: 0 0 8px 0; font-size: 20px; font-weight: bold;'>Inquiry Acknowledgment</h2>
                        <p style='margin: 0; font-size: 14px; opacity: 0.85;'>Hydroclimatic Sensing Research Lab (HSRL) — CEWRE, UET Lahore</p>
                    </div>
                    <div style='padding: 24px;'>
                        <p>Dear {inquiry.FullName},</p>
                        <p>Thank you for reaching out to the Hydroclimatic Sensing Research Lab (HSRL) at the Centre of Excellence in Water Resources Engineering (CEWRE), UET Lahore.</p>
                        <p>We have successfully received your inquiry regarding <strong>{inquiry.CollaborationType}</strong> (Reference Code: <strong>#HSRL-{inquiry.Id:D5}</strong>). Our research coordinator and faculty team will review your proposal and respond promptly.</p>
                        <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 20px 0;' />
                        <p style='font-size: 13px; color: #64748b;'>
                            <strong>Lab Affiliation:</strong> {siteConfig.Affiliation}<br />
                            <strong>Location:</strong> {siteConfig.Location}<br />
                            <strong>Official Email:</strong> {siteConfig.ContactEmail}
                        </p>
                    </div>
                </div>";

            await SendEmailAsync(inquiry.Email, subject, body);
        }
    }
}
