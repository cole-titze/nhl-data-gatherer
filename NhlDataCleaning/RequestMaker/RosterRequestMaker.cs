using System.Net.Http.Headers;
using Newtonsoft.Json;
using Entities.Models;

namespace NhlDataCleaning.RequestMaker
{
    public class RosterRequestMaker : IRosterRequestMaker
    {
        private const string _gameUrl = "http://statsapi.web.nhl.com/api/v1/game/";
        private const string _teamUrl = "http://statsapi.web.nhl.com/api/v1/teams/";
        private const string _teamQuery = "?expand=team.roster&season=";
        private const string _gameQuery = "/feed/live";
        public async Task<RosterIds> GetPlayerIds(CleanedGame game)
        {
            RosterIds ids;
            try
            {
                ids = await GamePlayerIdsGetter(game);
            }
            catch
            {
                ids = await SeasonPlayerIdsGetter(game);
            }

            return ids;
        }

        private async Task<RosterIds> SeasonPlayerIdsGetter(CleanedGame game)
        {
            var response = await MakeRosterRequest(game.homeTeamId, game.awayTeamId, game.seasonStartYear);
            var message = await ParseResponse(response);
            if (!response.IsSuccessStatusCode || !IsValidResponse(message))
                return new RosterIds();
            return GetIdsFromSeasonResponse(message);
        }

        private async Task<RosterIds> GamePlayerIdsGetter(CleanedGame game)
        {
            var response = await MakeRosterRequest(game.id.ToString() + _gameQuery);
            var message = await ParseResponse(response);
            if (!response.IsSuccessStatusCode || !IsValidResponse(message))
                throw new Exception("No roster available");
            return GetIdsFromGameResponse(response);
        }

        private async Task<dynamic> ParseResponse(HttpResponseMessage response)
        {
            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(data) ?? "";
        }

        private bool IsValidResponse(dynamic message)
        {
            if (message.liveData == null && message.teams == null)
                return false;
            return true;
        }

        private RosterIds GetIdsFromGameResponse(dynamic message)
        {
            if (message == null)
                return new RosterIds();

            return ParseGameMessageToPlayerIds(message);
        }

        private RosterIds ParseGameMessageToPlayerIds(dynamic message)
        {
            var ids = new RosterIds();
            var homeSkaters = message.liveData.boxscore.teams.home.skaters;
            foreach(int skaterId in homeSkaters)
            {
                ids.homeRosterIds.Add(skaterId);
            }
            int homeGoalie = message.liveData.boxscore.teams.home.goalies[0];
            ids.homeRosterIds.Add(homeGoalie);

            var awaySkaters = message.liveData.boxscore.teams.away.skaters;
            foreach(int skaterId in awaySkaters)
            {
                ids.awayRosterIds.Add(skaterId);
            }
            int awayGoalie = message.liveData.boxscore.teams.away.goalies[0];
            ids.awayRosterIds.Add(awayGoalie);

            return ids;
        }
        private RosterIds GetIdsFromSeasonResponse(dynamic message)
        {
            if (message == null)
                return new RosterIds();

            return ParseSeasonMessageToPlayerIds(message);
        }

        private RosterIds ParseSeasonMessageToPlayerIds(dynamic message)
        {
            var ids = new RosterIds();
            var homeTeam = message.teams[0];
            var awayTeam = message.teams[1];
            foreach(var player in homeTeam.roster.roster)
            {
                ids.homeRosterIds.Add((int)player.person.id);
            }
            foreach (var player in awayTeam.roster.roster)
            {
                ids.awayRosterIds.Add((int)player.person.id);
            }
            return ids;
        }

        public async Task<HttpResponseMessage> MakeRosterRequest(string query)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_gameUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                response = await client.GetAsync(query);
            }
            return response;
        }
        public async Task<HttpResponseMessage> MakeRosterRequest(int homeTeamId, int awayTeamId, int seasonStartYear)
        {
            var query = _teamQuery + seasonStartYear.ToString() + (seasonStartYear + 1).ToString() + "&teamId=" + homeTeamId + "," + awayTeamId;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_teamUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                response = await client.GetAsync(query);
            }
            return response;
        }
    }
}

