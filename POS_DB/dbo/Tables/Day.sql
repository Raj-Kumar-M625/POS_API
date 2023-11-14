CREATE TABLE [dbo].[Day] (
    [Id]          INT            NOT NULL,
    [Date]        DATETIME2 (7)  NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

