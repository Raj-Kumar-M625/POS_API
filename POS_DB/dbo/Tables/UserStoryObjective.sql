CREATE TABLE [dbo].[UserStoryObjective] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ObjectiveId] INT            NOT NULL,
    [UserStoryId] INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_UserStoryObjective] PRIMARY KEY CLUSTERED ([Id] ASC)
);

