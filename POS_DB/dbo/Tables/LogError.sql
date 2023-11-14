CREATE TABLE [dbo].[LogError] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ControllerName] VARCHAR (255) NULL,
    [MethodName]     VARCHAR (255) NULL,
    [LogDescription] VARCHAR (MAX) NULL,
    [TableName]      VARCHAR (255) NULL,
    [CreatedBy]      VARCHAR (255) NULL,
    [CreatedOn]      DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

