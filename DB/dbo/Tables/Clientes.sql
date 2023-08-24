CREATE TABLE [dbo].[Clientes] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Empresa]          NVARCHAR (250) NOT NULL,
    [Email]            NVARCHAR (250) NOT NULL,
    [Instagram]        NVARCHAR (250) NULL,
    [Web]              NVARCHAR (250) NULL,
    [Facebook]         NVARCHAR (250) NULL,
    [Linkedin]         NVARCHAR (250) NULL,
    [Twitter]          NVARCHAR (250) NULL,
    [CUIT]             VARCHAR (13)   NULL,
    [RazonSocial]      NVARCHAR (250) NULL,
    [DomicilioLegal]   NVARCHAR (250) NULL,
    [ReferenteNombre]  NVARCHAR (120) NOT NULL,
    [CelularReferente] VARCHAR (18)   NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

