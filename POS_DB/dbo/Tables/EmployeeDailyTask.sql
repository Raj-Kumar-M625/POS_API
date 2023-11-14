CREATE TABLE [dbo].[EmployeeDailyTask] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeId]         INT             NOT NULL,
    [EmployeeTaskId]     INT             NOT NULL,
    [ProjectId]          INT             NOT NULL,
    [Name]               NVARCHAR (MAX)  NOT NULL,
    [Description]        NVARCHAR (MAX)  NOT NULL,
    [StartDate]          DATETIME2 (7)   NOT NULL,
    [EndDate]            DATETIME2 (7)   NOT NULL,
    [EstTime]            DECIMAL (18, 2) NULL,
    [ActTime]            DECIMAL (18, 2) NULL,
    [WeekEndingDate]     DATETIME2 (7)   NOT NULL,
    [Status]             NVARCHAR (MAX)  NOT NULL,
    [Priority]           NVARCHAR (MAX)  NOT NULL,
    [Percentage]         INT             NOT NULL,
    [CreatedDate]        DATETIME2 (7)   NULL,
    [CreatedBy]          NVARCHAR (MAX)  NOT NULL,
    [UpdatedDate]        DATETIME2 (7)   NULL,
    [UpdatedBy]          NVARCHAR (MAX)  NOT NULL,
    [ProjectObjectiveId] INT             NULL,
    CONSTRAINT [PK_TimePlan] PRIMARY KEY CLUSTERED ([Id] ASC)
);

