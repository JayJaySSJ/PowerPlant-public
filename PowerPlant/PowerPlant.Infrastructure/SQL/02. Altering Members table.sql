
USE [PowerPlantDb];

DELETE FROM [Members];

ALTER TABLE [Members]
ALTER COLUMN [Function] INT NOT NULL;

INSERT INTO [Members] 
([Login], [Password], [Function])
VALUES ('admin', 'admin', '0');
