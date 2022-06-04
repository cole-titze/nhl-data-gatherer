namespace DataAccess.PlayerRepository
{
	public interface IPlayerRepository
	{
		Task<double> GetPlayerValueById(int id);
		Task<string> GetPositionById(int id);
		double GetPlayerValueByIdFromCache(int id);
		string GetPositionByIdFromCache(int id);
		Task CachePlayerValues();
	}
}

