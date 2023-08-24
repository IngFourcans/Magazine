CREATE TABLE [dbo].[RelClientesUsuarios] (
    [Usuario] INT NOT NULL,
    [Cliente] INT NOT NULL,
    CONSTRAINT [PK_RelClientesUsuarios] PRIMARY KEY CLUSTERED ([Usuario] ASC, [Cliente] ASC),
    CONSTRAINT [FK_RelClientesUsuarios_Clientes] FOREIGN KEY ([Cliente]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_RelClientesUsuarios_Usuarios] FOREIGN KEY ([Usuario]) REFERENCES [dbo].[Usuarios] ([Id])
);

