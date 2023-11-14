CREATE TABLE [dbo].[EmployeeProject] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  INT            NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [StartDate]   DATETIME2 (7)  NOT NULL,
    [EndDate]     DATETIME2 (7)  NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_EmployeeProject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

