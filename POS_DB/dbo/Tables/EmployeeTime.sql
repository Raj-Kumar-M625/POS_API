CREATE TABLE [dbo].[EmployeeTime] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  INT            NOT NULL,
    [InTime]      DATETIME2 (7)  NULL,
    [OutTime]     DATETIME2 (7)  NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_EmployeeLoginDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);

