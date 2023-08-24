CREATE TABLE [dbo].[Avisos] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [TituloSEO]          NVARCHAR (60)  NULL,
    [DescripcionSEO]     NVARCHAR (160) NULL,
    [RutaImagen]         NVARCHAR (500) NULL,
    [FechaActualizacion] DATE           NOT NULL,
    [FechaVencimiento]   DATE           NULL,
    [FechaBaja]          DATE           NULL,
    [Cliente]            INT            NOT NULL,
    [Categoria]          INT            NOT NULL,
    CONSTRAINT [PK_Avisos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Avisos_Categorias] FOREIGN KEY ([Categoria]) REFERENCES [dbo].[Categorias] ([Id]),
    CONSTRAINT [FK_Avisos_Clientes] FOREIGN KEY ([Cliente]) REFERENCES [dbo].[Clientes] ([Id])
);

