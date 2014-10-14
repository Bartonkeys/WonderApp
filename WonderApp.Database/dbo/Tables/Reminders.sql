CREATE TABLE [dbo].[Reminders] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Time]    NVARCHAR (MAX) NOT NULL,
    [User_Id] NVARCHAR (128) NULL,
    CONSTRAINT [PK_Reminders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserReminder] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserReminder]
    ON [dbo].[Reminders]([User_Id] ASC);

