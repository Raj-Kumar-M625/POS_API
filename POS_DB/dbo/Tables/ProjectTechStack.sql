CREATE TABLE [dbo].[ProjectTechStack] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [TechStack]   INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProjectTechStack] PRIMARY KEY CLUSTERED ([Id] ASC)
);

