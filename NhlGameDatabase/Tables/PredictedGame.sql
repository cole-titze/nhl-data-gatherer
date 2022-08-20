CREATE TABLE [dbo].[PredictedGame]
(
    id INT NOT NULL,
    homeTeamId INT NOT NULL,
    awayTeamId INT NOT NULL,
    gameDate Datetime NOT NULL,
    bovadaVegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    bovadaVegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    myBookieVegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    myBookieVegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    pinnacleVegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    pinnacleVegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    betOnlineVegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    betOnlineVegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    bet365VegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    bet365VegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    modelHomeOdds FLOAT NOT NULL DEFAULT 0,
    modelAwayOdds FLOAT NOT NULL DEFAULT 0,
    hasBeenPlayed BIT NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY (homeTeamId) REFERENCES Team(id),
    FOREIGN KEY (awayTeamId) REFERENCES Team(id)
);