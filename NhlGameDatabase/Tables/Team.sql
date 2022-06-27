CREATE TABLE [dbo].[Team]
(
    id INT NOT NULL,
    locationName VARCHAR(MAX) NOT NULL,
    teamName VARCHAR(MAX) NOT NULL,
    logoUri VARCHAR(MAX) NOT NULL,
    PRIMARY KEY(id),
    FOREIGN KEY (homeTeamId) REFERENCES Team(id),
    FOREIGN KEY (awayTeamId) REFERENCES Team(id),
);