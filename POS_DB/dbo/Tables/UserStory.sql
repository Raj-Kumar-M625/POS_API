CREATE TABLE [dbo].[UserStory] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]          INT            NOT NULL,
    [Name]               NVARCHAR (MAX) NOT NULL,
    [Description]        NVARCHAR (MAX) NOT NULL,
    [Status]             NVARCHAR (MAX) NOT NULL,
    [Percentage]         INT            NOT NULL,
    [StartDate]          DATETIME2 (7)  NOT NULL,
    [EndDate]            DATETIME2 (7)  NOT NULL,
    [CreatedDate]        DATETIME2 (7)  NULL,
    [CreatedBy]          NVARCHAR (MAX) NOT NULL,
    [UpdatedDate]        DATETIME2 (7)  NULL,
    [UpdatedBy]          NVARCHAR (MAX) NOT NULL,
    [ProjectObjectiveId] INT            NULL,
    CONSTRAINT [PK_UserStory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

