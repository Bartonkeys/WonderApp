CREATE TABLE [dbo].[UserDealReject] (
    [MyRejectUsers_Id] NVARCHAR (128) NOT NULL,
    [MyRejects_Id]     INT            NOT NULL,
    [Timestamp]        DATETIME       DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_UserDealReject] PRIMARY KEY CLUSTERED ([MyRejectUsers_Id] ASC, [MyRejects_Id] ASC),
    CONSTRAINT [FK_UserDealReject_AspNetUser] FOREIGN KEY ([MyRejectUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserDealReject_Deal] FOREIGN KEY ([MyRejects_Id]) REFERENCES [dbo].[Deals] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_UserDealReject_Deal]
    ON [dbo].[UserDealReject]([MyRejects_Id] ASC);

