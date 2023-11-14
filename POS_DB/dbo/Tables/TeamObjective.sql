CREATE TABLE [dbo].[TeamObjective] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TeamId]      INT            NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Status]      NVARCHAR (MAX) NULL,
    [Percentage]  INT            NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TeamObjective] PRIMARY KEY CLUSTERED ([Id] ASC)
);

