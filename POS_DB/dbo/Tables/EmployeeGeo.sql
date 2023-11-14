CREATE TABLE [dbo].[EmployeeGeo]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmployeeId] INT NOT NULL, 
    [DayId] INT NOT NULL, 
    [EmployeeTimeId] INT NOT NULL,
    [Latitude] DECIMAL(19, 9) NOT NULL, 
    [Longitude] DECIMAL(19, 9) NOT NULL, 
    [CreatedDate] DATETIME2 NULL, 
    [CreatedBy] NVARCHAR(MAX) NULL
)
