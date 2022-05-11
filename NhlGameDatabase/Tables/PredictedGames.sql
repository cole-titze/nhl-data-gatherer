CREATE TABLE [dbo].[PredictedGame]
(
    id INT NOT NULL,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    gameDate Datetime NOT NULL,
    vegasOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    modelOdds DECIMAL(4,1) NOT NULL DEFAULT 0,
    PRIMARY KEY(id),
);