CREATE TABLE [dbo].[FutureGame]
(
    id INT NOT NULL PRIMARY KEY,
    homeTeamId INT NOT NULL,
    awayTeamId INT NOT NULL,
    gameDate Datetime NOT NULL,
    FOREIGN KEY (homeTeamId) REFERENCES Team(id),
    FOREIGN KEY (awayTeamId) REFERENCES Team(id),
)
