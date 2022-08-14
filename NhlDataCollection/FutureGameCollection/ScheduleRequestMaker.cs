﻿using System.Net.Http.Headers;

namespace NhlDataCollection.FutureGameCollection
{
    public class ScheduleRequestMaker : IScheduleRequestMaker
    {
        private const string _url = "https://statsapi.web.nhl.com/api/v1/schedule?gameType=R";
        // Example Request: https://statsapi.web.nhl.com/api/v1/schedule?gameType=R&startDate=2022-04-29&endDate=2022-04-29
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
            var query = $"&startDate={dateString}&endDate={dateString}";

            return query;
        }

        public string CreateRequestQueryToGetTotalGames(int year)
        {
            int nextYear = year + 1;
            return $"?season={year}{nextYear}&gameType=R";
        }
    }
}