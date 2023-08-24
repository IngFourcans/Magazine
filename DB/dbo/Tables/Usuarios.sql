CREATE TABLE [dbo].[Usuarios] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]           NVARCHAR (250) NOT NULL,
    [Usuario]          VARCHAR (250)  NOT NULL,
    [TipoUsuario]      INT            NOT NULL,
    [Email]            NVARCHAR (250) NOT NULL,
    [EmailNormalizado] NVARCHAR (250) NULL,
    [PasswordHash]     NVARCHAR (MAX) NOT NULL,
    [EmailConfirmado]  BIT            CONSTRAINT [DF_Usuarios_EmailConfirmado] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Usuarios_TiposDeUsuarios] FOREIGN KEY ([TipoUsuario]) REFERENCES [dbo].[TiposDeUsuarios] ([Id])
);

