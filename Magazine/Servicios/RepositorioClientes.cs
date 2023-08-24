using Dapper;
using Magazine.Models;
using Microsoft.Data.SqlClient;

namespace Magazine.Servicios

{
    public interface IRepositorioClientes
    {
        Task Actualizar(Clientes cliente);
        Task ActualizarRubrosSeleccionados(List<int> rubrosseleccionados, int cliente);
        Task Borrar(int id);
        Task<Clientes> ClienteXId(int id, int usuarioId);
        Task Crear(Clientes clientes);
        Task<IEnumerable<Clientes>> Listar(int usuarioId);
        Task<IEnumerable<Clientes>> Listar(int cliente, int usuarioId);
        Task<IEnumerable<RubrosSeleccionados>> ObtenerRubrosxCliente(int id);
    }
    public class RepositorioClientes : IRepositorioClientes
    {
        private readonly string connectionString;

        public RepositorioClientes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Clientes>> Listar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Clientes>(@"SELECT c.Id, Empresa,Email, ReferenteNombre, CelularReferente,ISNULL(a.Id,-1) aviso
                                                            FROM [Magazine].[dbo].[Clientes] c
                                                           INNER JOIN RelClientesUsuarios u on c.Id=u.cliente
                                                           LEFT JOIN Avisos a ON c.Id=a.cliente
                                                           WHERE u.usuario=@usuarioId 
                                                            ORDER BY Empresa", new { usuarioId });

        }
        public async Task<IEnumerable<Clientes>> Listar(int cliente,int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Clientes>(@"SELECT c.Id, Empresa,Email, ReferenteNombre, CelularReferente,-1 aviso
                                                            FROM [Magazine].[dbo].[Clientes] c
                                                           INNER JOIN RelClientesUsuarios u on c.Id=u.cliente
                                                           WHERE u.usuario=@usuarioId AND  c.Id=@cliente
                                                            ORDER BY Empresa", new { usuarioId,cliente });

        }
        public async Task<Clientes> ClienteXId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Clientes>(@"SELECT Id,Empresa,Email,Instagram
                                                            ,Web,Facebook,Linkedin,Twitter
                                                            ,CUIT,RazonSocial,DomicilioLegal,ReferenteNombre
                                                            ,CelularReferente 
                                                            FROM [Magazine].[dbo].[Clientes] c
                                                            INNER JOIN RelClientesUsuarios u on c.Id = u.cliente
                                                            WHERE u.usuario = @usuarioId
                                                            AND c.Id=@id
                                                            ORDER BY Empresa", new { id, usuarioId });

        }
        
        
        public async Task Crear(Clientes clientes)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO [dbo].[Clientes]
                    ([Empresa],[Email],[Instagram],[Web],[Facebook],[Linkedin],[Twitter]
                    ,[CUIT],[RazonSocial],[DomicilioLegal],[ReferenteNombre],[CelularReferente])
                    VALUES (@Empresa,@Email,@Instagram,@Web,@Facebook,@Linkedin,@Twitter
                    ,@CUIT,@RazonSocial,@DomicilioLegal,@ReferenteNombre,@CelularReferente)
                    SELECT SCOPE_IDENTITY();", clientes);

            clientes.Id = id;
        }
        public async Task Actualizar(Clientes cliente)
        {
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE [dbo].[Clientes]
                SET [Empresa] = @Empresa,[Email] = @Email,[Instagram] = @Instagram,[Web] = @Web
                ,[Facebook] = @Facebook,[Linkedin] = @Linkedin,[Twitter] = @Twitter,[CUIT] = @CUIT
                ,[RazonSocial] = @RazonSocial,[DomicilioLegal] = @DomicilioLegal
                ,[ReferenteNombre] = @ReferenteNombre,[CelularReferente] = @CelularReferente
                WHERE Id=@id", cliente);
        }
        public async Task Borrar(int id)
        {
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("ClientesBorrar", new {id},commandType: System.Data.CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<RubrosSeleccionados>> ObtenerRubrosxCliente(int id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<RubrosSeleccionados>(@"SELECT r.id,Rubro,Orden, 
                                    CASE WHEN cr.IdCliente IS NOT NULL THEN 'true' ELSE 'false' END seleccionado 
                                    FROM Rubros r  WITH(NOLOCK) 
                                    LEFT JOIN RelClientesRubros cr WITH(NOLOCK) ON r.Id =cr.IdRubro AND cr.IdCliente = @id
                                    ORDER BY r.rubro", new { id });

        }
        public async Task ActualizarRubrosSeleccionados(List<int> rubrosseleccionados,int  cliente)
        {

            var connection = new SqlConnection(connectionString);
            var rs = "";
            foreach (var r in rubrosseleccionados)
            {
                rs = rs + r.ToString() + ",";
            }
            rs=rs.Substring(0, rs.Length - 1);
            await connection.ExecuteAsync("ActualizarRubrosSeleccionados",
                                                new { rubrosseleccionados = rs,
                                                cliente= cliente},
                                                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
