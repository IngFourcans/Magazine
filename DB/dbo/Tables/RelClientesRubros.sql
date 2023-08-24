CREATE TABLE [dbo].[RelClientesRubros] (
    [IdCliente] INT NOT NULL,
    [IdRubro]   INT NOT NULL,
    CONSTRAINT [PK_RelClientesRubros] PRIMARY KEY CLUSTERED ([IdCliente] ASC, [IdRubro] ASC),
    CONSTRAINT [FK_RelClientesRubros_Clientes] FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_RelClientesRubros_Rubros] FOREIGN KEY ([IdRubro]) REFERENCES [dbo].[Rubros] ([Id])
);

