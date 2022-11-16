
using Azure.Messaging.ServiceBus;
using SampleShared.Models;
using System.Text.Json;

const string connectionString = "Endpoint=sb://tafimessagebroker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=b7GR/SHkTKsrBPwKnOfdQXJNmxP8I4GPYp5E+4x832g=";

    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);

  
    
    ServiceBusProcessor processor = serviceBusClient.CreateProcessor("personque", new ServiceBusProcessorOptions());

    try
    {
        // add handler to process messages
        processor.ProcessMessageAsync += MessageHandler;

        // add handler to process any errors
        processor.ProcessErrorAsync += ErrorHandler;

        // start processing 
        await processor.StartProcessingAsync();
        
        Console.WriteLine("Wait for a minute and then press any key to end the processing");
        Console.ReadKey();

        // stop processing 
        Console.WriteLine("\nStopping the receiver...");
        await processor.StopProcessingAsync();
        Console.WriteLine("Stopped receiving messages");
    }
    finally
    {
        // Calling DisposeAsync on client types is required to ensure that network
        // resources and other unmanaged objects are properly cleaned up.
        await processor.DisposeAsync();
        await serviceBusClient.DisposeAsync();
    }

// handle received messages
static async Task MessageHandler(ProcessMessageEventArgs args)
{
    var msgBody  = args.Message.Body;
    var personObject = JsonSerializer.Deserialize<Person>(msgBody);

    Console.WriteLine($"First Name: {personObject?.FirstName}");
    Console.WriteLine($"Last Name: {personObject?.LastName}");
    Console.WriteLine($"Email: {personObject?.Email}");

    // complete the message. messages is deleted from the queue. 
    await args.CompleteMessageAsync(args.Message);
}


// handle any errors when receiving messages
static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}