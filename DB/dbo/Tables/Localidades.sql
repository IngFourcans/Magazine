CREATE TABLE [dbo].[Localidades] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Localidad] NVARCHAR (250) NOT NULL,
    [Provincia] INT            NOT NULL,
    CONSTRAINT [PK_Localidades] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Localidades_Provincias] FOREIGN KEY ([Provincia]) REFERENCES [dbo].[Provincias] ([Id])
);

