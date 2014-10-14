CREATE TABLE [dbo].[Companies] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (MAX) NOT NULL,
    [Address]    NVARCHAR (MAX) NOT NULL,
    [PostCode]   NVARCHAR (MAX) NOT NULL,
    [County]     NVARCHAR (MAX) NOT NULL,
    [Phone]      NVARCHAR (MAX) NOT NULL,
    [Country_Id] INT            NULL,
    [CityId]     INT            CONSTRAINT [DF_Companies_CityId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Companies_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id]),
    CONSTRAINT [FK_CompanyCountry] FOREIGN KEY ([Country_Id]) REFERENCES [dbo].[Countries] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_CompanyCountry]
    ON [dbo].[Companies]([Country_Id] ASC);

