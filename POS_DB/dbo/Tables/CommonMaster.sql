CREATE TABLE [dbo].[CommonMaster] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CodeType]        NVARCHAR (50) NULL,
    [CodeName]        NVARCHAR (50) NULL,
    [CodeValue]       NVARCHAR (50) NULL,
    [DisplaySequence] INT           NULL,
    [IsActive]        BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

