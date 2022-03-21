namespace NhlDataCollection.DataGetter
{
    public interface IRequestMaker
    {
        Task<HttpResponseMessage> MakeRequest(string query);
        string CreateRequestQuery(int year, int id);
        int BuildId(int year, int id);
    }
}