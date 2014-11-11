CREATE TABLE [dbo].[DealAge] (
    [Deals_Id] INT NOT NULL,
    [Ages_Id]  INT NOT NULL,
    CONSTRAINT [PK_DealAge] PRIMARY KEY CLUSTERED ([Deals_Id] ASC, [Ages_Id] ASC),
    CONSTRAINT [FK_DealAge_Age] FOREIGN KEY ([Ages_Id]) REFERENCES [dbo].[Ages] ([Id]),
    CONSTRAINT [FK_DealAge_Deal] FOREIGN KEY ([Deals_Id]) REFERENCES [dbo].[Deals] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealAge_Age]
    ON [dbo].[DealAge]([Ages_Id] ASC);

