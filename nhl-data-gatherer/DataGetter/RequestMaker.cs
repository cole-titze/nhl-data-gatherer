using System.Net.Http.Headers;

namespace nhl_data_builder.DataGetter
{
    public class RequestMaker : IRequestMaker
	{
        private const string _seasonType = "02";
        private const string _url = "http://statsapi.web.nhl.com/api/v1/game/";
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
            // Build request url
            string urlParameters = year.ToString() + _seasonType + "000" + id.ToString() + "/feed/live";

            return urlParameters;
        }
	}
}