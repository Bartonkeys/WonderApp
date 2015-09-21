CREATE TABLE [dbo].[Addresses] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AddressLine1] NVARCHAR (200) NULL,
    [AddressLine2] NVARCHAR (200) NULL,
    [PostCode]     NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC)
);



