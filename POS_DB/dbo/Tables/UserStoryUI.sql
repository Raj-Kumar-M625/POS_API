CREATE TABLE [dbo].[UserStoryUI] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UIId]        INT            NULL,
    [UserStoryId] INT            NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_UIUserStory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

