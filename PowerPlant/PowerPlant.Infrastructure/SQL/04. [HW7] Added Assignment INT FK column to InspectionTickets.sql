USE [PowerPlantDb];

  ALTER TABLE [InspectionTickets]
  ADD [Assignment] INT FOREIGN KEY ([Assignment]) REFERENCES [Members]([Id]);