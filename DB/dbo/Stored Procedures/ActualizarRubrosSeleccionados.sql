


CREATE PROCEDURE [dbo].[ActualizarRubrosSeleccionados]
    @rubrosseleccionados NVARCHAR(MAX),
    @cliente INT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @script AS NVARCHAR(MAX)


	SET @script = N'DELETE FROM RelClientesRubros WHERE idcliente=@cliente 
					INSERT INTO [dbo].RelClientesRubros (IdCliente, IdRubro)
					SELECT @cliente, r.Id FROM Rubros r WHERE r.id IN  (' + @rubrosseleccionados + N')'
	
	-- Ejecutar el script de inserción
	EXEC sp_executesql @script, N'@cliente INT', @cliente
END