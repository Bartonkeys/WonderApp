CREATE TABLE [dbo].[UserPreferences] (
    [UserId]         NVARCHAR (128) NOT NULL,
    [ReminderId]     INT            NULL,
    [EmailMyWonders] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserPreferences_1] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_UserPreferences_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserPreferences_Reminders] FOREIGN KEY ([ReminderId]) REFERENCES [dbo].[Reminders] ([Id])
);





