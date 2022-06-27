CREATE TABLE [dbo].[PredictedGame]
(
    id INT NOT NULL,
    homeTeamId INT NOT NULL,
    awayId INT NOT NULL,
    gameDate Datetime NOT NULL,
    vegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    vegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    modelHomeOdds FLOAT NOT NULL DEFAULT 0,
    modelAwayOdds FLOAT NOT NULL DEFAULT 0,
    PRIMARY KEY(id),
    FOREIGN KEY (homeTeamId) REFERENCES Team(id),
    FOREIGN KEY (awayTeamId) REFERENCES Team(id)
);