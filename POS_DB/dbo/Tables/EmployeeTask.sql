CREATE TABLE [dbo].[EmployeeTask] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeId]         INT             NOT NULL,
    [TaskId]             INT             NOT NULL,
    [ProjectId]          INT             NOT NULL,
    [StartDate]          DATETIME2 (7)   NULL,
    [EndDate]            DATETIME2 (7)   NULL,
    [EstTime]            DECIMAL (18, 2) NOT NULL,
    [ActTime]            DECIMAL (18, 2) NULL,
    [WeekEndingDate]     DATETIME2 (7)   NOT NULL,
    [Status]             NVARCHAR (MAX)  NOT NULL,
    [Priority]           NVARCHAR (MAX)  NOT NULL,
    [Percentage]         INT             NOT NULL,
    [CreatedDate]        DATETIME2 (7)   NULL,
    [CreatedBy]          NVARCHAR (MAX)  NOT NULL,
    [UpdatedDate]        DATETIME2 (7)   NULL,
    [UpdatedBy]          NVARCHAR (MAX)  NOT NULL,
    [EstStartDate]       DATETIME2 (7)   NOT NULL,
    [EstEndDate]         DATETIME2 (7)   NOT NULL,
    [ProjectObjectiveId] INT             NULL,
    [DayId]              INT             NULL,
    CONSTRAINT [PK_DayPlan] PRIMARY KEY CLUSTERED ([Id] ASC)
);

