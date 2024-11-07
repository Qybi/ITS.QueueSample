// receiver
// testing out popping messages from the queue

using Azure.Storage.Queues;
using ITS.QueueSample.Models;
using System.Text.Json;

var queueClient = new QueueClient("UseDevelopmentStorage=true", "its-chat");

await queueClient.CreateIfNotExistsAsync();
await queueClient.ClearMessagesAsync();


while (true)
{
    var messages = await queueClient.ReceiveMessagesAsync();
    if (messages.Value.Length == 0)
    {
        await Task.Delay(1000);
        continue;
    }
    foreach (var message in messages.Value)
    {
        var m = JsonSerializer.Deserialize<SampleMessage>(message.MessageText);
        if (m == null) continue;
        //Console.ForegroundColor = m.Color;
        //Console.WriteLine($"Received: {m.Text} sent on {m.CreationDate}");
        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
    }
}