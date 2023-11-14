CREATE TABLE [dbo].[Comments] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]           INT            NULL,
    [TaskId]              INT            NULL,
    [EmployeeTaskId]      INT            NULL,
    [EmployeeDailyTaskId] INT            NULL,
    [EmployeeId]          INT            NULL,
    [EmployeeTimeId]      INT            NULL,
    [Comment]             NVARCHAR (MAX) NOT NULL,
    [CreatedDate]         DATETIME2 (7)  NULL,
    [CreatedBy]           NVARCHAR (MAX) NOT NULL,
    [UpdatedDate]         DATETIME2 (7)  NULL,
    [UpdatedBy]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

