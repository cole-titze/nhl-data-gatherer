CREATE TABLE [dbo].[PredictedGame]
(
    id INT NOT NULL,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    gameDate Datetime NOT NULL,
    vegasHomeOdds FLOAT NOT NULL DEFAULT 0,
    vegasAwayOdds FLOAT NOT NULL DEFAULT 0,
    modelHomeOdds FLOAT NOT NULL DEFAULT 0,
    modelAwayOdds FLOAT NOT NULL DEFAULT 0,
    PRIMARY KEY(id),
);