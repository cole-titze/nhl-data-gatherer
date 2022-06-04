using System.Net.Http.Headers;
using Newtonsoft.Json;
using Entities.Models;

namespace NhlDataCleaning.RequestMaker
{
    public class RosterRequestMaker : IRosterRequestMaker
    {
        private const string _url = "http://statsapi.web.nhl.com/api/v1/game/";
        private const string _query = "/feed/live";
        public async Task<RosterIds> GetPlayerIds(int gameid)
        {
            var response = await MakeRosterRequest(gameid.ToString() + _query);
            if (!response.IsSuccessStatusCode)
                return new RosterIds();

            var ids = await GetIdsFromResponse(response);
            return ids;
        }

        private async Task<RosterIds> GetIdsFromResponse(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (message == null)
                return new RosterIds();

            RosterIds ids = ParseMessageToPlayerIds(message);
            return ids;
        }

        private RosterIds ParseMessageToPlayerIds(dynamic message)
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

        public async Task<HttpResponseMessage> MakeRosterRequest(string query)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                response = await client.GetAsync(query);
            }
            return response;
        }

        public Task<List<int>> GetHomePlayerIds(int year)
        {
            throw new NotImplementedException();
        }
    }
}

