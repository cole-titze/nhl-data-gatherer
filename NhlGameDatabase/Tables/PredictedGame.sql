CREATE TABLE [dbo].[PredictedGame]
(
    id INT NOT NULL,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    gameDate Datetime NOT NULL,
    vegasHomeOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    vegasAwayOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    modelHomeOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    modelAwayOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    PRIMARY KEY(id),
);