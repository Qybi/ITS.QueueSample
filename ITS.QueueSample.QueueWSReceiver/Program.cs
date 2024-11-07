using ITS.QueueSample.DataAccess;
using ITS.QueueSample.IDataAccess;
using ITS.QueueSample.QueueWSReceiver;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IProductsDataService>(dataAccess => new ProductsDataService(builder.Configuration));

var host = builder.Build();
host.Run();
