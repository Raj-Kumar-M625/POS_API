CREATE TABLE [dbo].[TeamProject] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TeamId]      INT            NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [StartDate]   DATETIME2 (7)  NOT NULL,
    [EndDate]     DATETIME2 (7)  NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TeamProject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

