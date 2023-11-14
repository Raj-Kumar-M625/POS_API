CREATE TABLE [dbo].[TeamWeeklyObjective]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TeamId] INT NOT NULL,
	[ProjectId] INT NOT NULL,
	[Name] NVARCHAR (MAX)  NOT NULL,
    [Description] NVARCHAR (MAX)  NOT NULL,
    [Priority] NVARCHAR (MAX)  NOT NULL,
	[Status] NVARCHAR (MAX)  NOT NULL,
    [Percentage] INT NOT NULL,
	[WeekEndingDate] DATETIME2 (7)   NOT NULL,
	[CreatedDate] DATETIME2 (7)   NULL,
    [CreatedBy] NVARCHAR (MAX)  NOT NULL,
    [UpdatedDate] DATETIME2 (7)   NULL,
    [UpdatedBy] NVARCHAR (MAX)  NOT NULL,
)
