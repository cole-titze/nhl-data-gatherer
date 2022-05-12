﻿CREATE TABLE [dbo].[CleanedGame]
(
    id INT NOT NULL,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    seasonStartYear INT NOT NULL,
    gameDate Datetime NOT NULL,
    homeWinRatio DECIMAL(6, 3) NOT NULL,
    homeRecentWinRatio DECIMAL(6, 3) NOT NULL,
    homeRecentGoalsAvg DECIMAL(6, 3) NOT NULL,
    homeRecentConcededGoalsAvg DECIMAL(6, 3) NOT NULL,
    homeRecentSogAvg DECIMAL(6, 3) NOT NULL,
    homeRecentPpgAvg DECIMAL(6, 3) NOT NULL,
    homeRecentHitsAvg DECIMAL(6, 3) NOT NULL,
    homeRecentPimAvg DECIMAL(6, 3) NOT NULL,
    homeRecentBlockedShotsAvg DECIMAL(6, 3) NOT NULL,
    homeRecentTakeawaysAvg DECIMAL(6, 3) NOT NULL,
    homeRecentGiveawaysAvg DECIMAL(6, 3) NOT NULL,
    homeGoalsAvg DECIMAL(6, 3) NOT NULL,
    homeGoalsAvgAtHome DECIMAL(6, 3) NOT NULL,
    homeRecentGoalsAvgAtHome DECIMAL(6, 3) NOT NULL,
    homeConcededGoalsAvg DECIMAL(6, 3) NOT NULL,
    homeConcededGoalsAvgAtHome DECIMAL(6, 3) NOT NULL,
    homeRecentConcededGoalsAvgAtHome DECIMAL(6, 3) NOT NULL,
    awayWinRatio DECIMAL(6, 3) NOT NULL,
    awayRecentWinRatio DECIMAL(6, 3) NOT NULL,
    awayRecentGoalsAvg DECIMAL(6, 3) NOT NULL,
    awayRecentConcededGoalsAvg DECIMAL(6, 3) NOT NULL,
    awayRecentSogAvg DECIMAL(6, 3) NOT NULL,
    awayRecentPpgAvg DECIMAL(6, 3) NOT NULL,
    awayRecentHitsAvg DECIMAL(6, 3) NOT NULL,
    awayRecentPimAvg DECIMAL(6, 3) NOT NULL,
    awayRecentBlockedShotsAvg DECIMAL(6, 3) NOT NULL,
    awayRecentTakeawaysAvg DECIMAL(6, 3) NOT NULL,
    awayRecentGiveawaysAvg DECIMAL(6, 3) NOT NULL,
    awayGoalsAvg DECIMAL(6, 3) NOT NULL,
    awayGoalsAvgAtAway DECIMAL(6, 3) NOT NULL,
    awayRecentGoalsAvgAtAway DECIMAL(6, 3) NOT NULL,
    awayConcededGoalsAvg DECIMAL(6, 3) NOT NULL,
    awayConcededGoalsAvgAtAway DECIMAL(6, 3) NOT NULL,
    awayRecentConcededGoalsAvgAtAway DECIMAL(6, 3) NOT NULL,
    winner INT NOT NULL,
    isExcluded BIT NOT NULL,
    PRIMARY KEY(id),
);