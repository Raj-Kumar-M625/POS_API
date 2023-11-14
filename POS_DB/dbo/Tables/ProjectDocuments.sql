CREATE TABLE [dbo].[ProjectDocuments] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ProjectId]   INT            NOT NULL,
    [DocName]     NVARCHAR (MAX) NOT NULL,
    [DocPath]     NVARCHAR (MAX) NOT NULL,
    [DocLink]     NVARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProjectDocuments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

