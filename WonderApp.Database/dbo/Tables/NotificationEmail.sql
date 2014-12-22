CREATE TABLE [dbo].[NotificationEmail] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Created]        DATETIME       NULL,
    [Sent]           DATETIME       NULL,
    [RecipientEmail] NVARCHAR (MAX) NULL,
    [RecipientName]  NVARCHAR (MAX) NULL,
    [Template_Id]    INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NotificationEmail_ToTemplate] FOREIGN KEY ([Template_Id]) REFERENCES [dbo].[Template] ([Id])
);

