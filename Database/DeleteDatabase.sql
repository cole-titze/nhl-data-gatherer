USE master;
GO

ALTER DATABASE Games SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE Games;
GO