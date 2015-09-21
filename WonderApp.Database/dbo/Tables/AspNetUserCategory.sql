CREATE TABLE [dbo].[AspNetUserCategory] (
    [UserId]        NVARCHAR (128) NOT NULL,
    [Categories_Id] INT            NOT NULL,
    CONSTRAINT [PK_AspNetUserCategory] PRIMARY KEY CLUSTERED ([UserId] ASC, [Categories_Id] ASC),
    CONSTRAINT [FK_AspNetUserCategory_AspNetUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_AspNetUserCategory_Category] FOREIGN KEY ([Categories_Id]) REFERENCES [dbo].[Categories] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserCategory_Category]
    ON [dbo].[AspNetUserCategory]([Categories_Id] ASC);

