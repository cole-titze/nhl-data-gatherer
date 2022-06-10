namespace NhlDataCollection.GameCollection
{
    public interface IGameRequestMaker
    {
        string BuildQueryByYear(int year);
        Task<string> MakeRequest(string query);
    }
}