CREATE PROCEDURE [dbo].[GET_ALL_GAMES]  
AS  
   BEGIN  
   SELECT 
    id,
    homeTeamName,
    awayTeamName,
    seasonStartYear,
    gameDate,
    homeGoals,
    awayGoals,
    winner,
    homeSOG,
    awaySOG,
    homePPG,
    awayPPG,
    homePIM,
    awayPIM,
    homeFaceOffWinPercent,
    awayFaceOffWinPercent,
    homeBlockedShots,
    awayBlockedShots,
    homeHits,
    awayHits,
    homeTakeaways,
    awayTakeaways,
    homeGiveaways,
    awayGiveaways

   FROM Game
END