using System;
using NhlDataCollection.DataAccess;
using NhlDataCollection.DataGetter;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Channel;

namespace DataCollectionTrigger
{
    public class DataCollection
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            // Setup Logging
            string appInsightsKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

            using var channel = new InMemoryChannel();
            IServiceCollection services = new ServiceCollection();
            services.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = channel);
            services.AddLogging(builder =>
            {
                // Only Application Insights is registered as a logger provider
                builder.AddApplicationInsights(appInsightsKey);
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ILogger<DataGetter> logger = serviceProvider.GetRequiredService<ILogger<DataGetter>>();


            // Run Application
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);

            var gameParser = new GameParser();
            var requestMaker = new RequestMaker();
            var dataAccess = new GamesDA(connectionString);
            var endYear = GetEndSeason(DateTime.UtcNow);
            var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, endYear, logger);
            await dataGetter.GetData();
            Console.WriteLine("Completed!");
        }

        // Season spans 2 years (2021-2022) but we only want the start year of the season
        // (ex. February 2022 we want 2021 to be the end season)
        public int GetEndSeason(DateTime currentDate)
        {
            var endSeasonDate = new DateTime(currentDate.Year, 09, 15);
            int currentSeasonYear;

            if (currentDate > endSeasonDate)
                currentSeasonYear = currentDate.Year;
            else
                currentSeasonYear = currentDate.Year - 1;

            return currentSeasonYear;
        }
    }
}