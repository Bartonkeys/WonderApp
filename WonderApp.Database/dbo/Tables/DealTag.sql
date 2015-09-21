CREATE TABLE [dbo].[DealTag] (
    [Deals_Id] INT NOT NULL,
    [Tags_Id]  INT NOT NULL,
    CONSTRAINT [PK_DealTag] PRIMARY KEY CLUSTERED ([Deals_Id] ASC, [Tags_Id] ASC),
    CONSTRAINT [FK_DealTag_Deal] FOREIGN KEY ([Deals_Id]) REFERENCES [dbo].[Deals] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_DealTag_Tag]
    ON [dbo].[DealTag]([Tags_Id] ASC);

