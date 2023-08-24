
CREATE PROCEDURE [dbo].ClientesBorrar
	@id int
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DELETE RelClientesRubros WHERE IdCliente=@id
	DELETE RElClientesUsuarios WHERE Cliente=@id
	DELETE Avisos WHERE Cliente=@id
	DELETE Sucursales WHERE Cliente=@id
	DELETE Clientes WHERE Id=@id
	
END
