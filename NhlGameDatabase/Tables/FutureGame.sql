CREATE TABLE [dbo].[FutureGame]
(
    id INT NOT NULL PRIMARY KEY,
    homeTeamName VARCHAR(MAX) NOT NULL,
    awayTeamName VARCHAR(MAX) NOT NULL,
    gameDate Datetime NOT NULL,
)
