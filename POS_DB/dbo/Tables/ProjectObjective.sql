CREATE TABLE [dbo].[ProjectObjective] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Status]      NVARCHAR (MAX) NOT NULL,
    [Percentage]  INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProjectObjective] PRIMARY KEY CLUSTERED ([Id] ASC)
);

