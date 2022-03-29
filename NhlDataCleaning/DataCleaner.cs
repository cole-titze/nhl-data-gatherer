using Entities;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IGamesDA _gamesDA;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;

        public DataCleaner(ILogger logger, IGamesDA gamesDa, DateRange dateRange)
        {
            _logger = logger;
            _gamesDA = gamesDa;
            _yearRange = dateRange;
        }
        public void CleanData()
        {
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                _logger.LogInformation("Cleaning Year: " + year.ToString());
                _gamesDA.CacheSeasonOfGames(year);
                var seasonsGames = _gamesDA.GetCachedGames();

            }
        }
    }
}
