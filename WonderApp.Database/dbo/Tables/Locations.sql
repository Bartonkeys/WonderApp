CREATE TABLE [dbo].[Locations] (
    [Id]            INT               IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX)    NOT NULL,
    [AspNetUser_Id] NVARCHAR (128)    NULL,
    [Geography]     [sys].[geography] NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserLocation] FOREIGN KEY ([AspNetUser_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserLocation]
    ON [dbo].[Locations]([AspNetUser_Id] ASC);

