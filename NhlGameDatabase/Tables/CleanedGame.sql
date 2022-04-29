﻿CREATE TABLE [dbo].[CleanedGame]
(
    id INT NOT NULL,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    seasonStartYear INT NOT NULL,
    gameDate Datetime NOT NULL,
    homeWinRatio DECIMAL(4, 1) NOT NULL,
    homeRecentWinRatio DECIMAL(4, 1) NOT NULL,
    homeRecentGoalsAvg DECIMAL(4, 1) NOT NULL,
    homeRecentConcededGoalsAvg DECIMAL(4, 1) NOT NULL,
    homeRecentSogAvg DECIMAL(4, 1) NOT NULL,
    homeRecentPpgAvg DECIMAL(4, 1) NOT NULL,
    homeRecentHitsAvg DECIMAL(4, 1) NOT NULL,
    homeRecentPimAvg DECIMAL(4, 1) NOT NULL,
    homeRecentBlockedShotsAvg DECIMAL(4, 1) NOT NULL,
    homeRecentTakeawaysAvg DECIMAL(4, 1) NOT NULL,
    homeRecentGiveawaysAvg DECIMAL(4, 1) NOT NULL,
    homeGoalsAvg DECIMAL(4, 1) NOT NULL,
    homeGoalsAvgAtHome DECIMAL(4, 1) NOT NULL,
    homeRecentGoalsAvgAtHome DECIMAL(4, 1) NOT NULL,
    homeConcededGoalsAvg DECIMAL(4, 1) NOT NULL,
    homeConcededGoalsAvgAtHome DECIMAL(4, 1) NOT NULL,
    homeRecentConcededGoalsAvgAtHome DECIMAL(4, 1) NOT NULL,
    awayWinRatio DECIMAL(4, 1) NOT NULL,
    awayRecentWinRatio DECIMAL(4, 1) NOT NULL,
    awayRecentGoalsAvg DECIMAL(4, 1) NOT NULL,
    awayRecentConcededGoalsAvg DECIMAL(4, 1) NOT NULL,
    awayRecentSogAvg DECIMAL(4, 1) NOT NULL,
    awayRecentPpgAvg DECIMAL(4, 1) NOT NULL,
    awayRecentHitsAvg DECIMAL(4, 1) NOT NULL,
    awayRecentPimAvg DECIMAL(4, 1) NOT NULL,
    awayRecentBlockedShotsAvg DECIMAL(4, 1) NOT NULL,
    awayRecentTakeawaysAvg DECIMAL(4, 1) NOT NULL,
    awayRecentGiveawaysAvg DECIMAL(4, 1) NOT NULL,
    awayGoalsAvg DECIMAL(4, 1) NOT NULL,
    awayGoalsAvgAtAway DECIMAL(4, 1) NOT NULL,
    awayRecentGoalsAvgAtAway DECIMAL(4, 1) NOT NULL,
    awayConcededGoalsAvg DECIMAL(4, 1) NOT NULL,
    awayConcededGoalsAvgAtAway DECIMAL(4, 1) NOT NULL,
    awayRecentConcededGoalsAvgAtAway DECIMAL(4, 1) NOT NULL,
    winner INT NOT NULL,
    isExcluded BIT NOT NULL,
    PRIMARY KEY(id),
);