CREATE TABLE [dbo].[Devices] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Type] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED ([Id] ASC)
);

