CREATE TABLE [dbo].[Tags] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_Tags_Name_Clustered]
    ON [dbo].[Tags]([Name] ASC);

