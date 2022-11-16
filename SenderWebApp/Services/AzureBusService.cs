using Azure.Messaging.ServiceBus;
using SampleShared.Models;
using System.Text.Json;

namespace SenderWebApp.Services
{
    public class AzureBusService : IAzureBusService
    {
        private readonly IConfiguration _config;

        public AzureBusService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            var connectionString = _config.GetConnectionString("AzureServiceBusConnection");

            var serviceBusClient = new ServiceBusClient(connectionString);

            var sender = serviceBusClient.CreateSender(queueName);

            var msgBody = JsonSerializer.Serialize(personMessage);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            bool IsMessagedAdded = messageBatch.TryAddMessage(new ServiceBusMessage(msgBody));

            if (!IsMessagedAdded)
            {
                throw new Exception($"The message is too large to fit in the batch.");
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);              
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await serviceBusClient.DisposeAsync();
            }


        }
    }
}
