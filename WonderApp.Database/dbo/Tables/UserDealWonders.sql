CREATE TABLE [dbo].[UserDealWonders] (
    [MyWonderUsers_Id] NVARCHAR (128) NOT NULL,
    [MyWonders_Id]     INT            NOT NULL,
    [Timestamp]        DATETIME       CONSTRAINT [DF__UserDealW__Times__5812160E] DEFAULT (getdate()) NULL,
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_UserDealWonders_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserDealWonders_AspNetUser] FOREIGN KEY ([MyWonderUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserDealWonders_Deal] FOREIGN KEY ([MyWonders_Id]) REFERENCES [dbo].[Deals] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_FK_UserDealWonders_Deal]
    ON [dbo].[UserDealWonders]([MyWonders_Id] ASC);

