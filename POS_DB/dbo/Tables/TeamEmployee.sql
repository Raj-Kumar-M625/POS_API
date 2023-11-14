CREATE TABLE [dbo].[TeamEmployee] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TeamId]      INT            NOT NULL,
    [EmployeeId]  INT            NOT NULL,
    [LeadId]      INT            NULL,
    [StartDate]   DATETIME2 (7)  NOT NULL,
    [EndDate]     DATETIME2 (7)  NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TeamEmployee] PRIMARY KEY CLUSTERED ([Id] ASC)
);

