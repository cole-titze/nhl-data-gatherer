﻿using Entities.Models;
using Newtonsoft.Json;

namespace NhlDataCollection.FutureGameCollection
{
    public class ScheduleParser : IScheduleParser
    {
        private const int DefaultGameCount = 1400;
        public async Task<List<Game>> BuildFutureGames(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidGame(message))
                return new List<Game>();

            List<Game> games = ParseMessageToGames(message);
            return games;
        }
        private List<Game> ParseMessageToGames(dynamic message)
        {
            var gameList = new List<Game>();
            foreach (var date in message.dates)
            {
                foreach (var game in date.games)
                {
                    var futureGame = new Game()
                    {
                        id = (int)game.gamePk,
                        seasonStartYear = int.Parse(((string)game.season).Substring(0, 4)),
                        homeTeamId = (int)game.teams.home.team.id,
                        awayTeamId = (int)game.teams.away.team.id,
                        gameDate = DateTime.Parse((string)game.gameDate),
                        hasBeenPlayed=false,
                    };
                    gameList.Add(futureGame);
                }
            }

            return gameList;
        }
        private bool InvalidGame(dynamic message)
        {
            if (message == null)
                return true;
            return (int)message.totalItems == 0;
        }

        public async Task<int> GetNumberOfGamesInSeason(HttpResponseMessage response)
        {
            // Give a default in case request fails
            if (!response.IsSuccessStatusCode)
                return DefaultGameCount;

            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (message == null)
                return DefaultGameCount;

            return Convert.ToInt32(message.totalItems);
        }
    }
}
