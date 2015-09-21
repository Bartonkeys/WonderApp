CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [Gender_Id]            INT            NULL,
    [Name]                 NVARCHAR (512) NULL,
    [Forename]             NVARCHAR (512) NULL,
    [Surname]              NVARCHAR (512) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserGender] FOREIGN KEY ([Gender_Id]) REFERENCES [dbo].[Genders] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserGender]
    ON [dbo].[AspNetUsers]([Gender_Id] ASC);

