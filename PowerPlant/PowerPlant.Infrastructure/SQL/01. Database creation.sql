CREATE DATABASE [PowerPlantDb];

USE [PowerPlantDb];

CREATE TABLE [Members] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Login] VARCHAR (255) UNIQUE NOT NULL,
	[Password] VARCHAR(255) NOT NULL,
	[Function] VARCHAR(255) NOT NULL);

INSERT INTO [Members] 
([Login], [Password], [Function])
VALUES ('admin', 'admin', 'Admin');

--SPRAWDè T• DAT  JESZCZE !
create table [CriticalReadings] (
	[PlantName] VARCHAR(255) NOT NULL, 
	[ItemName] VARCHAR(255) NOT NULL, 
	[ParameterName] VARCHAR(255) NOT NULL, 
	[ReadingTime] DATETIME2, 
	[LoggedUserLogin] VARCHAR(255) NOT NULL, 
	[MinValue] FLOAT(8) NOT NULL, 
	[MaxValue] FLOAT(8) NOT NULL);