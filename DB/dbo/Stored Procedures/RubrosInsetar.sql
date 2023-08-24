
CREATE PROCEDURE RubrosInsetar
	@Rubro nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @Orden int

	SELECT @Orden = COALESCE(Max(Orden),0)+1 FROM Rubros
	
	INSERT INTO [dbo].[Rubros]
           ([Rubro]
           ,[Orden])
     VALUES
           (@Rubro
           ,@Orden)
	SELECT SCOPE_IDENTITY();
END
