CREATE TABLE [dbo].[Template] (
    [Id]        INT           NOT NULL,
    [Name]      NCHAR (200)   NULL,
    [Thumbnail] NVARCHAR (20) NULL,
    [File]      NVARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

