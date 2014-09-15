
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/15/2014 09:00:51
-- Generated from EDMX file: C:\WonderApp\WonderApp\WonderApp.Data\WonderAppModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WonderApp];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DealCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Deals] DROP CONSTRAINT [FK_DealCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCountry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_CompanyCountry];
GO
IF OBJECT_ID(N'[dbo].[FK_DealTag_Deal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DealTag] DROP CONSTRAINT [FK_DealTag_Deal];
GO
IF OBJECT_ID(N'[dbo].[FK_DealTag_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DealTag] DROP CONSTRAINT [FK_DealTag_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_DealLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Deals] DROP CONSTRAINT [FK_DealLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_DealCost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Deals] DROP CONSTRAINT [FK_DealCost];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserGender]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUsers] DROP CONSTRAINT [FK_AspNetUserGender];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDealWonders_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDealWonders] DROP CONSTRAINT [FK_UserDealWonders_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDealWonders_Deal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDealWonders] DROP CONSTRAINT [FK_UserDealWonders_Deal];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDealReject_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDealReject] DROP CONSTRAINT [FK_UserDealReject_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDealReject_Deal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDealReject] DROP CONSTRAINT [FK_UserDealReject_Deal];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserReminder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reminders] DROP CONSTRAINT [FK_AspNetUserReminder];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_AspNetUserLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserCategory_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserCategory] DROP CONSTRAINT [FK_AspNetUserCategory_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserCategory_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserCategory] DROP CONSTRAINT [FK_AspNetUserCategory_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryDeal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Deals] DROP CONSTRAINT [FK_CategoryDeal];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Deals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Deals];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Reminders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reminders];
GO
IF OBJECT_ID(N'[dbo].[Costs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Costs];
GO
IF OBJECT_ID(N'[dbo].[Genders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genders];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[DealTag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DealTag];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[UserDealWonders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDealWonders];
GO
IF OBJECT_ID(N'[dbo].[UserDealReject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDealReject];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserCategory];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Deals'
CREATE TABLE [dbo].[Deals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [ExpiryDate] datetime  NOT NULL,
    [Likes] int  NOT NULL,
    [Publish] bit  NOT NULL,
    [Company_Id] int  NOT NULL,
    [Location_Id] int  NOT NULL,
    [Cost_Id] int  NULL,
    [Category_Id] int  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Longitude] decimal(18,0)  NOT NULL,
    [Latitude] decimal(18,0)  NOT NULL,
    [AspNetUser_Id] nvarchar(128)  NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [PostCode] nvarchar(max)  NOT NULL,
    [County] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [Country_Id] int  NULL
);
GO

-- Creating table 'Reminders'
CREATE TABLE [dbo].[Reminders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Time] nvarchar(max)  NOT NULL,
    [User_Id] nvarchar(128)  NULL
);
GO

-- Creating table 'Costs'
CREATE TABLE [dbo].[Costs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Range] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Genders'
CREATE TABLE [dbo].[Genders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL,
    [Gender_Id] int  NULL
);
GO

-- Creating table 'Images'
CREATE TABLE [dbo].[Images] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [Deal_Id] int  NOT NULL,
    [Device_Id] int  NOT NULL
);
GO

-- Creating table 'Devices'
CREATE TABLE [dbo].[Devices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DealTag'
CREATE TABLE [dbo].[DealTag] (
    [Deals_Id] int  NOT NULL,
    [Tags_Id] int  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [RoleId] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserDealWonders'
CREATE TABLE [dbo].[UserDealWonders] (
    [MyWonderUsers_Id] nvarchar(128)  NOT NULL,
    [MyWonders_Id] int  NOT NULL
);
GO

-- Creating table 'UserDealReject'
CREATE TABLE [dbo].[UserDealReject] (
    [MyRejectUsers_Id] nvarchar(128)  NOT NULL,
    [MyRejects_Id] int  NOT NULL
);
GO

-- Creating table 'AspNetUserCategory'
CREATE TABLE [dbo].[AspNetUserCategory] (
    [UserId] nvarchar(128)  NOT NULL,
    [Categories_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Deals'
ALTER TABLE [dbo].[Deals]
ADD CONSTRAINT [PK_Deals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reminders'
ALTER TABLE [dbo].[Reminders]
ADD CONSTRAINT [PK_Reminders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Costs'
ALTER TABLE [dbo].[Costs]
ADD CONSTRAINT [PK_Costs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Genders'
ALTER TABLE [dbo].[Genders]
ADD CONSTRAINT [PK_Genders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [PK_Images]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [PK_Devices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Deals_Id], [Tags_Id] in table 'DealTag'
ALTER TABLE [dbo].[DealTag]
ADD CONSTRAINT [PK_DealTag]
    PRIMARY KEY CLUSTERED ([Deals_Id], [Tags_Id] ASC);
GO

-- Creating primary key on [RoleId], [UserId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([RoleId], [UserId] ASC);
GO

-- Creating primary key on [MyWonderUsers_Id], [MyWonders_Id] in table 'UserDealWonders'
ALTER TABLE [dbo].[UserDealWonders]
ADD CONSTRAINT [PK_UserDealWonders]
    PRIMARY KEY CLUSTERED ([MyWonderUsers_Id], [MyWonders_Id] ASC);
GO

-- Creating primary key on [MyRejectUsers_Id], [MyRejects_Id] in table 'UserDealReject'
ALTER TABLE [dbo].[UserDealReject]
ADD CONSTRAINT [PK_UserDealReject]
    PRIMARY KEY CLUSTERED ([MyRejectUsers_Id], [MyRejects_Id] ASC);
GO

-- Creating primary key on [UserId], [Categories_Id] in table 'AspNetUserCategory'
ALTER TABLE [dbo].[AspNetUserCategory]
ADD CONSTRAINT [PK_AspNetUserCategory]
    PRIMARY KEY CLUSTERED ([UserId], [Categories_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Company_Id] in table 'Deals'
ALTER TABLE [dbo].[Deals]
ADD CONSTRAINT [FK_DealCompany]
    FOREIGN KEY ([Company_Id])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DealCompany'
CREATE INDEX [IX_FK_DealCompany]
ON [dbo].[Deals]
    ([Company_Id]);
GO

-- Creating foreign key on [Country_Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_CompanyCountry]
    FOREIGN KEY ([Country_Id])
    REFERENCES [dbo].[Countries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCountry'
CREATE INDEX [IX_FK_CompanyCountry]
ON [dbo].[Companies]
    ([Country_Id]);
GO

-- Creating foreign key on [Deals_Id] in table 'DealTag'
ALTER TABLE [dbo].[DealTag]
ADD CONSTRAINT [FK_DealTag_Deal]
    FOREIGN KEY ([Deals_Id])
    REFERENCES [dbo].[Deals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Id] in table 'DealTag'
ALTER TABLE [dbo].[DealTag]
ADD CONSTRAINT [FK_DealTag_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DealTag_Tag'
CREATE INDEX [IX_FK_DealTag_Tag]
ON [dbo].[DealTag]
    ([Tags_Id]);
GO

-- Creating foreign key on [Location_Id] in table 'Deals'
ALTER TABLE [dbo].[Deals]
ADD CONSTRAINT [FK_DealLocation]
    FOREIGN KEY ([Location_Id])
    REFERENCES [dbo].[Locations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DealLocation'
CREATE INDEX [IX_FK_DealLocation]
ON [dbo].[Deals]
    ([Location_Id]);
GO

-- Creating foreign key on [Cost_Id] in table 'Deals'
ALTER TABLE [dbo].[Deals]
ADD CONSTRAINT [FK_DealCost]
    FOREIGN KEY ([Cost_Id])
    REFERENCES [dbo].[Costs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DealCost'
CREATE INDEX [IX_FK_DealCost]
ON [dbo].[Deals]
    ([Cost_Id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [RoleId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUser]
ON [dbo].[AspNetUserRoles]
    ([UserId]);
GO

-- Creating foreign key on [Gender_Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [FK_AspNetUserGender]
    FOREIGN KEY ([Gender_Id])
    REFERENCES [dbo].[Genders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserGender'
CREATE INDEX [IX_FK_AspNetUserGender]
ON [dbo].[AspNetUsers]
    ([Gender_Id]);
GO

-- Creating foreign key on [MyWonderUsers_Id] in table 'UserDealWonders'
ALTER TABLE [dbo].[UserDealWonders]
ADD CONSTRAINT [FK_UserDealWonders_AspNetUser]
    FOREIGN KEY ([MyWonderUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MyWonders_Id] in table 'UserDealWonders'
ALTER TABLE [dbo].[UserDealWonders]
ADD CONSTRAINT [FK_UserDealWonders_Deal]
    FOREIGN KEY ([MyWonders_Id])
    REFERENCES [dbo].[Deals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDealWonders_Deal'
CREATE INDEX [IX_FK_UserDealWonders_Deal]
ON [dbo].[UserDealWonders]
    ([MyWonders_Id]);
GO

-- Creating foreign key on [MyRejectUsers_Id] in table 'UserDealReject'
ALTER TABLE [dbo].[UserDealReject]
ADD CONSTRAINT [FK_UserDealReject_AspNetUser]
    FOREIGN KEY ([MyRejectUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MyRejects_Id] in table 'UserDealReject'
ALTER TABLE [dbo].[UserDealReject]
ADD CONSTRAINT [FK_UserDealReject_Deal]
    FOREIGN KEY ([MyRejects_Id])
    REFERENCES [dbo].[Deals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDealReject_Deal'
CREATE INDEX [IX_FK_UserDealReject_Deal]
ON [dbo].[UserDealReject]
    ([MyRejects_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Reminders'
ALTER TABLE [dbo].[Reminders]
ADD CONSTRAINT [FK_AspNetUserReminder]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserReminder'
CREATE INDEX [IX_FK_AspNetUserReminder]
ON [dbo].[Reminders]
    ([User_Id]);
GO

-- Creating foreign key on [AspNetUser_Id] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [FK_AspNetUserLocation]
    FOREIGN KEY ([AspNetUser_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserLocation'
CREATE INDEX [IX_FK_AspNetUserLocation]
ON [dbo].[Locations]
    ([AspNetUser_Id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserCategory'
ALTER TABLE [dbo].[AspNetUserCategory]
ADD CONSTRAINT [FK_AspNetUserCategory_AspNetUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_Id] in table 'AspNetUserCategory'
ALTER TABLE [dbo].[AspNetUserCategory]
ADD CONSTRAINT [FK_AspNetUserCategory_Category]
    FOREIGN KEY ([Categories_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserCategory_Category'
CREATE INDEX [IX_FK_AspNetUserCategory_Category]
ON [dbo].[AspNetUserCategory]
    ([Categories_Id]);
GO

-- Creating foreign key on [Category_Id] in table 'Deals'
ALTER TABLE [dbo].[Deals]
ADD CONSTRAINT [FK_CategoryDeal]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryDeal'
CREATE INDEX [IX_FK_CategoryDeal]
ON [dbo].[Deals]
    ([Category_Id]);
GO

-- Creating foreign key on [Deal_Id] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [FK_DealImage]
    FOREIGN KEY ([Deal_Id])
    REFERENCES [dbo].[Deals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DealImage'
CREATE INDEX [IX_FK_DealImage]
ON [dbo].[Images]
    ([Deal_Id]);
GO

-- Creating foreign key on [Device_Id] in table 'Images'
ALTER TABLE [dbo].[Images]
ADD CONSTRAINT [FK_DeviceImage]
    FOREIGN KEY ([Device_Id])
    REFERENCES [dbo].[Devices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceImage'
CREATE INDEX [IX_FK_DeviceImage]
ON [dbo].[Images]
    ([Device_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------