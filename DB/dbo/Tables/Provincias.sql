CREATE TABLE [dbo].[Provincias] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Provincia] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_Provincias] PRIMARY KEY CLUSTERED ([Id] ASC)
);

