using System.Net.Http.Headers;

namespace NhlDataCollection.GameCollection
{
    public class GameRequestMaker : IGameRequestMaker
	{
        private const string _url = "http://statsapi.web.nhl.com/api/v1/schedule?season=";
        // Example Request: http://statsapi.web.nhl.com/api/v1/schedule?season=20172018
        public async Task<string> MakeRequest(string query)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                response = await client.GetAsync(query);
            }
            if (!response.IsSuccessStatusCode)
                return string.Empty;

            return await response.Content.ReadAsStringAsync();
        }
        public string BuildQueryByYear(int year)
        {
            int futureYear = year + 1;
            string yearStr = year.ToString() + futureYear.ToString();

            return _url + yearStr;
        }
    }
}