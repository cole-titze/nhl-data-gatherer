CREATE TABLE [dbo].[Team]
(
    id INT NOT NULL,
    locationName VARCHAR(MAX) NOT NULL,
    teamName VARCHAR(MAX) NOT NULL,
    logoUri VARCHAR(MAX) NOT NULL,
    PRIMARY KEY(id),
);