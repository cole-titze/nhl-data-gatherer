namespace NhlDataCollection.FutureGameCollection
{
    public interface IScheduleRequestMaker
    {
        string CreateRequestQuery(DateTime futureDate, DateTime today);
        Task<HttpResponseMessage> MakeRequest(string query);
        string CreateRequestQueryToGetTotalGames(int year);
    }
}