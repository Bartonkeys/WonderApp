CREATE TABLE [dbo].[Images] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [url]       NVARCHAR (MAX) NOT NULL,
    [Deal_Id]   INT            NOT NULL,
    [Device_Id] INT            NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DealImage] FOREIGN KEY ([Deal_Id]) REFERENCES [dbo].[Deals] ([Id]),
    CONSTRAINT [FK_DeviceImage] FOREIGN KEY ([Device_Id]) REFERENCES [dbo].[Devices] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealImage]
    ON [dbo].[Images]([Deal_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DeviceImage]
    ON [dbo].[Images]([Device_Id] ASC);

