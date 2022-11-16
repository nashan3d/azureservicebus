using SampleShared.Models;

namespace SenderWebApp.Services
{
    public interface IAzureBusService
    {
        Task SendMessageAsync(Person personMessage, string queueName);
    }
}
