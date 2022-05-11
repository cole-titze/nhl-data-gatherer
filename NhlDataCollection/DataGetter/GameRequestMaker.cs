using System.Net.Http.Headers;

namespace NhlDataCollection.DataGetter
{
    public class GameRequestMaker : IGameRequestMaker
	{
        private const string _seasonType = "02";
        private const string _url = "http://statsapi.web.nhl.com/api/v1/game/";
        // Example Request: http://statsapi.web.nhl.com/api/v1/game/2019020001/feed/live
        public async Task<HttpResponseMessage> MakeRequest(string query)
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

        public string CreateRequestQuery(int year, int id)
        {
            var idStr = BuildId(year, id);
            // Build request url
            string urlParameters = $"{idStr}/feed/live";

            return urlParameters;
        }
        public int BuildId(int year, int id)
        {
            var yearStr = year.ToString();
            var idStr = String.Format("{0,0:D4}", id);
            return Convert.ToInt32($"{yearStr}{_seasonType}{idStr}");
        }
    }
}