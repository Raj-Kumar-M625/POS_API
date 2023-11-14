CREATE TABLE [dbo].[Employee] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserId]       INT            NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [PhoneNumber]  NVARCHAR (MAX) NOT NULL,
    [Category]     NVARCHAR (MAX) NULL,
    [IsActive]     BIT            NOT NULL,
    [ProfilePhoto] NVARCHAR (MAX) NOT NULL,
    [Department]   NVARCHAR (MAX) NULL,
    [CreatedDate]  DATETIME2 (7)  NULL,
    [CreatedBy]    NVARCHAR (MAX) NOT NULL,
    [UpdatedDate]  DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC)
);

