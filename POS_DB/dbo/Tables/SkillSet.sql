CREATE TABLE [dbo].[SkillSet] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Category]     NVARCHAR (MAX) NOT NULL,
    [CreatedDate]  DATETIME2 (7)  NULL,
    [CreatedBy]    NVARCHAR (MAX) NOT NULL,
    [UpdatedDate]  DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (MAX) NOT NULL,
    [SubCategory1] NVARCHAR (255) DEFAULT ('default_value') NOT NULL,
    [SubCategory2] NVARCHAR (255) NULL,
    [SubCategory3] NVARCHAR (255) NULL,
    CONSTRAINT [PK_SkillSet] PRIMARY KEY CLUSTERED ([Id] ASC)
);

