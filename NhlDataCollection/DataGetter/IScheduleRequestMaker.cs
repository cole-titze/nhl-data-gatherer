namespace NhlDataCollection.DataGetter
{
    public interface IScheduleRequestMaker
    {
        string CreateRequestQuery(DateTime tomorrow);
        Task<HttpResponseMessage> MakeRequest(string query);
    }
}