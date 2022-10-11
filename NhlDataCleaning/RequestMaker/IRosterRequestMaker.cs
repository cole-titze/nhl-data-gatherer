using Entities.Models;

namespace NhlDataCleaning.RequestMaker
{
	public interface IRosterRequestMaker
	{
		Task<RosterIds> GetPlayerIds(CleanedGame game);
	}
}

