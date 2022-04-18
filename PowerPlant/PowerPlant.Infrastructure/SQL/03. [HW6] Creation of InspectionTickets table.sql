USE [PowerPlantDb];

CREATE TABLE [InspectionTickets] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[CreationDate] DATETIME2 NOT NULL,
	[AssignmentDate] DATETIME2,
	[TerminationDate] DATETIME2,
	[ItemName] VARCHAR(255) NOT NULL, 
	[Comment] VARCHAR(255),
	[Status] INT
	);