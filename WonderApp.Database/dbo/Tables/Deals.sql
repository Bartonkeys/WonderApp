CREATE TABLE [dbo].[Deals] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (MAX) NOT NULL,
    [Description]      NVARCHAR (MAX) NOT NULL,
    [Url]              NVARCHAR (MAX) NOT NULL,
    [ExpiryDate]       DATETIME       NOT NULL,
    [Likes]            INT            NOT NULL,
    [Publish]          BIT            NOT NULL,
    [Company_Id]       INT            NOT NULL,
    [Location_Id]      INT            NOT NULL,
    [Cost_Id]          INT            NULL,
    [Category_Id]      INT            NULL,
    [Archived]         BIT            CONSTRAINT [DF_Deals_Archived] DEFAULT ((0)) NULL,
    [IntroDescription] NVARCHAR (MAX) NULL,
    [Priority]         BIT            NULL,
    [CityId]           INT            CONSTRAINT [DF_Deals_CityId] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Deals] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CategoryDeal] FOREIGN KEY ([Category_Id]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_DealCompany] FOREIGN KEY ([Company_Id]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_DealCost] FOREIGN KEY ([Cost_Id]) REFERENCES [dbo].[Costs] ([Id]),
    CONSTRAINT [FK_DealLocation] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[Locations] ([Id]),
    CONSTRAINT [FK_Deals_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_CategoryDeal]
    ON [dbo].[Deals]([Category_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealCompany]
    ON [dbo].[Deals]([Company_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealCost]
    ON [dbo].[Deals]([Cost_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealLocation]
    ON [dbo].[Deals]([Location_Id] ASC);

