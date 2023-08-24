CREATE TABLE [dbo].[Sucursales] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [NombreSucursal]       NVARCHAR (250) NOT NULL,
    [SucursalCentral]      TINYINT        CONSTRAINT [DF_Domicilios_SucursalCentral] DEFAULT ((0)) NULL,
    [Calle]                NVARCHAR (250) NULL,
    [Nro]                  NVARCHAR (10)  NULL,
    [Piso]                 NVARCHAR (10)  NULL,
    [Departamento]         NVARCHAR (10)  NULL,
    [InformacionAdicional] NVARCHAR (250) NULL,
    [Telefono]             NVARCHAR (18)  NULL,
    [Whatsapp]             NVARCHAR (18)  NULL,
    [Provincia]            INT            NOT NULL,
    [Localidad]            INT            NOT NULL,
    [Cliente]              INT            NOT NULL,
    CONSTRAINT [PK_Domicilios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Domicilios_Clientes] FOREIGN KEY ([Cliente]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_Sucursales_Localidades] FOREIGN KEY ([Localidad]) REFERENCES [dbo].[Localidades] ([Id]),
    CONSTRAINT [FK_Sucursales_Provincias] FOREIGN KEY ([Provincia]) REFERENCES [dbo].[Provincias] ([Id])
);

