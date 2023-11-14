CREATE TABLE [dbo].[Team] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [StartDate]   DATETIME2 (7)  NOT NULL,
    [EndDate]     DATETIME2 (7)  NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED ([Id] ASC)
);

