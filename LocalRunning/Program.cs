using DataCollectionTrigger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var collector = new DataCollection();

// Build service collection
var collection = new ServiceCollection();
var sp = collection.BuildServiceProvider();

// Get logger and run main
using (var scope = sp.CreateScope())
{
    string? gamesConnectionString = Environment.GetEnvironmentVariable("GAMES_DATABASE");
    string? playersConnectionString = Environment.GetEnvironmentVariable("PLAYERS_DATABASE");

    if( gamesConnectionString == null || playersConnectionString == null)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Local.json").Build();
        gamesConnectionString = config.GetConnectionString("GAMES_DATABASE");
        playersConnectionString = config.GetConnectionString("PLAYERS_DATABASE");
    }
    if (gamesConnectionString == null || playersConnectionString == null)
        throw new Exception("Connection String Null");

     await collector.Main(gamesConnectionString, playersConnectionString);
}