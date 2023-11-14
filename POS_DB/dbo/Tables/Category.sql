CREATE TABLE [dbo].[Category] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Categories]          NVARCHAR (MAX) NOT NULL,
    [SubCategory]         NVARCHAR (MAX) NOT NULL,
    [UiApplicable]        BIT            NOT NULL,
    [UserStoryApplicable] BIT            NOT NULL,
    [CreatedDate]         DATETIME2 (7)  NULL,
    [CreatedBy]           NVARCHAR (MAX) NOT NULL,
    [UpdatedDate]         DATETIME2 (7)  NULL,
    [UpdatedBy]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

