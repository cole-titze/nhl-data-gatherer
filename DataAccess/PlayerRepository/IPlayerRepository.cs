namespace DataAccess.PlayerRepository
{
	public interface IPlayerRepository
	{
		Task<double> GetPlayerValueById(int id);
	}
}

