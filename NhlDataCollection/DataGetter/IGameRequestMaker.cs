namespace NhlDataCollection.DataGetter
{
    public interface IGameRequestMaker
    {
        int BuildId(int year, int id);
        string CreateRequestQuery(int year, int id);
        Task<HttpResponseMessage> MakeRequest(string query);
    }
}