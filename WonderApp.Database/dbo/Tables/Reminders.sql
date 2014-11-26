CREATE TABLE [dbo].[Reminders] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Time] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Reminders] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO


