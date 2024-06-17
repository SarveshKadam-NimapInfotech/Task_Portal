
namespace Task_Portal.Services.Email
{
    public interface IEmailService
    {
        System.Threading.Tasks.Task SendEmailAsync(string to, string subject, string body);
    }
}
