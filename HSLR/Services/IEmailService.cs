using HSLR.Models.Entities;

namespace HSLR.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent, string? replyTo = null);
        Task SendInquiryNotificationAsync(CollaborationInquiry inquiry, SiteConfig siteConfig);
        Task SendInquiryConfirmationAsync(CollaborationInquiry inquiry, SiteConfig siteConfig);
    }
}
