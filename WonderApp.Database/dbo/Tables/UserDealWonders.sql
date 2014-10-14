CREATE TABLE [dbo].[UserDealWonders] (
    [MyWonderUsers_Id] NVARCHAR (128) NOT NULL,
    [MyWonders_Id]     INT            NOT NULL,
    CONSTRAINT [PK_UserDealWonders] PRIMARY KEY CLUSTERED ([MyWonderUsers_Id] ASC, [MyWonders_Id] ASC),
    CONSTRAINT [FK_UserDealWonders_AspNetUser] FOREIGN KEY ([MyWonderUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserDealWonders_Deal] FOREIGN KEY ([MyWonders_Id]) REFERENCES [dbo].[Deals] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserDealWonders_Deal]
    ON [dbo].[UserDealWonders]([MyWonders_Id] ASC);

