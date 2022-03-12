namespace nhl_data_builder.DataGetter
{
    public interface IRequestMaker
    {
        Task<HttpResponseMessage> MakeRequest(string query);
        string CreateRequestQuery(int year, int id);
    }
}