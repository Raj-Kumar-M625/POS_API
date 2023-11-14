CREATE TABLE [dbo].[EmployeeDay] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeId]  INT            NULL,
    [DayId]       INT            NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([DayId]) REFERENCES [dbo].[Day] ([Id]),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([Id])
);

