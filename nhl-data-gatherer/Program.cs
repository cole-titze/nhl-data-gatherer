using Microsoft.Extensions.Configuration;
using nhl_data_builder.DataGetter;
using nhl_data_gatherer.DataAccess;

// Setup Database Configuration
var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true);
IConfiguration config = builder.Build();

// Run Application
var gameParser = new GameParser();
var requestMaker = new RequestMaker();
var dataAccess = new GamesDA(config);
var endYear = DateTime.Now.Year;
var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, endYear);
await dataGetter.GetData();
