CREATE TABLE [dbo].[Rubros] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Rubro] NVARCHAR (50) NULL,
    [Orden] INT           NULL,
    CONSTRAINT [PK_Rubros] PRIMARY KEY CLUSTERED ([Id] ASC)
);

