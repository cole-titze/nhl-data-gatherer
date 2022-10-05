CREATE TABLE [dbo].[LogLossGame]
(
    id INT NOT NULL,
    bovadaLogLoss FLOAT NOT NULL DEFAULT 0,
    myBookieLogLoss FLOAT NOT NULL DEFAULT 0,
    pinnacleLogLoss FLOAT NOT NULL DEFAULT 0,
    betOnlineLogLoss FLOAT NOT NULL DEFAULT 0,
    bet365LogLoss FLOAT NOT NULL DEFAULT 0,
    modelLogLoss FLOAT NOT NULL DEFAULT 0,
    PRIMARY KEY(id),
);