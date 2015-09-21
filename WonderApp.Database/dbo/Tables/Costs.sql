CREATE TABLE [dbo].[Costs] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Range] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Costs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

