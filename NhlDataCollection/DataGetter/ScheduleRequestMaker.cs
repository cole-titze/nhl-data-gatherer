using System.Net.Http.Headers;

namespace NhlDataCollection.DataGetter
{
    public class ScheduleRequestMaker : IScheduleRequestMaker
    {
        private const string _url = "https://statsapi.web.nhl.com/api/v1/schedule";
        // Example Request: https://statsapi.web.nhl.com/api/v1/schedule?startDate=2022-04-29&endDate=2022-04-29
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

        public string CreateRequestQuery(DateTime tomorrow)
        {
            var dateString = $"{tomorrow.Year}-{tomorrow.Month}-{tomorrow.Day}";
            var query = $"?startDate={dateString}&endDate={dateString}";

            return query;
        }

        public string CreateRequestQuery(int year, int id)
        {
            throw new NotImplementedException();
        }
    }
}