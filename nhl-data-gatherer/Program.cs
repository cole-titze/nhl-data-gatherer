using Microsoft.Extensions.Configuration;
using nhl_data_builder.DataGetter;

// Setup Database Configuration
var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);
IConfiguration config = builder.Build();

// Run Application
var gameParser = new GameParser();
var requestMaker = new RequestMaker();
var dataGetter = new DataGetter(gameParser, requestMaker, config);
await dataGetter.GetData();
