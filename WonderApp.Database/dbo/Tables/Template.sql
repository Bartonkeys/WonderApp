CREATE TABLE [dbo].[Template] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NCHAR (200)    NULL,
    [Thumbnail] NVARCHAR (200) NULL,
    [File]      NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



