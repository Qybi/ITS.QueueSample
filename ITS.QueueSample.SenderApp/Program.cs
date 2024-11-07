// sender
using Azure.Storage.Queues;
using ITS.QueueSample.Models;
using Spectre.Console;
using System.Text.Json;

var queueClient = new QueueClient("UseDevelopmentStorage=true", "its-chat");

await queueClient.CreateIfNotExistsAsync();

while (true)
{
    AnsiConsole.MarkupLine("[magenta]Welcome to the products DB explorer:[/]");
    var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What do you want to do?")
                .PageSize(10)
                .AddChoices(Enum.GetValues<CRUDAction>().Select(x => x.ToString()))
        );

    Enum.TryParse(choice, true, out CRUDAction action);
    SampleMessage m = new SampleMessage();
    switch (action)
    {
        case CRUDAction.Create:
            {
                var p = new Product();
                p.Name = AnsiConsole.Ask<string>("Product name:");
                p.Code = AnsiConsole.Ask<string>("Product code:");
                p.Price = AnsiConsole.Ask<decimal>("Product price:");

                m.Product = p;
                m.CreationDate = DateTime.Now;
                m.Action = action;
            }
            break;
        //case CRUDAction.Read:
        //    break;
        //case CRUDAction.ReadAll:
        //    break;
        case CRUDAction.Update:
            {
                var id = AnsiConsole.Ask<int>("Product ID to update:");
                var p = new Product();
                p.Name = AnsiConsole.Ask<string>("Product name:");
                p.Code = AnsiConsole.Ask<string>("Product code:");
                p.Price = AnsiConsole.Ask<decimal>("Product price:");


                m = new SampleMessage()
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = "Updated name",
                        Code = "Updated code",
                        Price = 100
                    },
                    CreationDate = DateTime.Now,
                    Action = action
                };
            }
            break;
        case CRUDAction.Delete:
            {
                var id = AnsiConsole.Ask<int>("Product ID to update:");

                m = new SampleMessage()
                {
                    Product = new Product()
                    {
                        Id = id
                    },
                    CreationDate = DateTime.Now,
                    Action = action
                };
            }
            break;
        default:
            break;
    }

    if (m == null)
    {
        continue;
    }

    var json = JsonSerializer.Serialize(m);

    await queueClient.SendMessageAsync(json);

    Console.WriteLine("Message sent!");
}