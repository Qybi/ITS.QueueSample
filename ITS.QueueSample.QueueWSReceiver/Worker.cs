using Azure.Storage.Queues;
using ITS.QueueSample.IDataAccess;
using ITS.QueueSample.Models;
using System.Text.Json;

namespace ITS.QueueSample.QueueWSReceiver;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IProductsDataService _productsDataService;

    public Worker(ILogger<Worker> logger, IProductsDataService productsDataService)
    {
        _logger = logger;
        _productsDataService = productsDataService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await QueueReceiver(stoppingToken);
    }
    private async Task QueueReceiver(CancellationToken stoppingToken)
    {
        var queueClient = new QueueClient("UseDevelopmentStorage=true", "its-chat");

        await queueClient.CreateIfNotExistsAsync();
        await queueClient.ClearMessagesAsync();

        while (!stoppingToken.IsCancellationRequested)
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

                switch (m.Action)
                {
                    case CRUDAction.Create:
                        await _productsDataService.InsertAsync(m.Product);
                        _logger.LogInformation("Product created: {product}", m.Product);
                        break;
                    case CRUDAction.Update:
                        await _productsDataService.UpdateAsync(m.Product);
                        _logger.LogInformation("Product updated: {product}", m.Product);
                        break;
                    case CRUDAction.Delete:
                        await _productsDataService.DeleteAsync(m.Product.Id);
                        _logger.LogInformation("Product deleted: {product}", m.Product);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }
    }
}
