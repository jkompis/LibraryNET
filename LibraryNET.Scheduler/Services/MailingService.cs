using System.Threading.Tasks;

namespace LibraryNET.Scheduler.Services;

interface IMailingService
{
    Task SendEmail(string email, string subject, string body);
}

internal sealed class MailingService : IMailingService
{
    public Task SendEmail(string email, string subject, string body)
    {
        return Task.CompletedTask;
    }
}