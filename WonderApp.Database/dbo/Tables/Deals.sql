﻿CREATE TABLE [dbo].[Deals] (
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
    [AlwaysAvailable]  BIT            NULL,
    [AddressId]        INT            NULL,
    [Creator_User_Id]  NVARCHAR (128) NULL,
    [Season_Id]        INT            NULL,
    [Phone]            NVARCHAR (200) NULL,
    [Gender_Id]        INT            NULL,
    [Expired]          BIT            NULL,
    [Broadcast]        BIT            NULL,
    CONSTRAINT [PK_Deals] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CategoryDeal] FOREIGN KEY ([Category_Id]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Deal_Creator] FOREIGN KEY ([Creator_User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_DealCompany] FOREIGN KEY ([Company_Id]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_DealCost] FOREIGN KEY ([Cost_Id]) REFERENCES [dbo].[Costs] ([Id]),
    CONSTRAINT [FK_DealLocation] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[Locations] ([Id]),
    CONSTRAINT [FK_Deals_Addresses] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Addresses] ([Id]),
    CONSTRAINT [FK_Deals_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id]),
    CONSTRAINT [FK_Deals_Genders] FOREIGN KEY ([Gender_Id]) REFERENCES [dbo].[Genders] ([Id]),
    CONSTRAINT [FK_Deals_Seasons] FOREIGN KEY ([Season_Id]) REFERENCES [dbo].[Seasons] ([Id])
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

