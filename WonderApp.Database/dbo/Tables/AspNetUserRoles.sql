CREATE TABLE [dbo].[AspNetUserRoles] (
    [RoleId] NVARCHAR (128) NOT NULL,
    [UserId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([RoleId] ASC, [UserId] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserRoles_AspNetUser]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);

