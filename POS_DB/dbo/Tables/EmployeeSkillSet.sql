CREATE TABLE [dbo].[EmployeeSkillSet] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SkillSetId]  INT            NOT NULL,
    [EmployeeId]  INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (MAX) NOT NULL,
    [UpdatedDate] DATETIME2 (7)  NULL,
    [UpdatedBy]   NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_EmployeeSkillSet] PRIMARY KEY CLUSTERED ([Id] ASC)
);

