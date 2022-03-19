using System.Net.Http.Headers;

namespace nhl_data_builder.DataGetter
{
    public class RequestMaker : IRequestMaker
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
            string idString = String.Format("{0,0:D4}", id);
            var yearStr = year.ToString();
            // Build request url
            string urlParameters = $"{yearStr}{_seasonType}{idString}/feed/live";

            return urlParameters;
        }
	}
}