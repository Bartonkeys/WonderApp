CREATE TABLE [dbo].[Ages] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Ages] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_Ages_Name_Clustered]
    ON [dbo].[Ages]([Name] ASC);

