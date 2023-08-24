using Dapper;
using Magazine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Magazine.Servicios
{
    public interface IRepositorioAvisos
    {
        Task Actualizar(Avisos avisos);
        Task<Avisos> AvisosPorId(int id, int usuarioId);
        Task Borrar(Avisos avisos);
        Task Crear(Avisos avisos);
        Task<Avisos> ExisteAvisoxUsuarioxCliente(int usuarioid, int clienteid);
        Task<IEnumerable<Avisos>> Listar(int usuarioId);
    }
    public class RepositorioAvisos: IRepositorioAvisos
    {
        private readonly string connectionstring;

        public RepositorioAvisos(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Avisos avisos)
        {
            var connection = new SqlConnection(connectionstring);

            var id = await connection.QuerySingleAsync<int> (@"INSERT INTO [dbo].[Avisos] ([TituloSEO],[DescripcionSEO],
                    [RutaImagen],[FechaActualizacion],[FechaVencimiento],[FechaBaja],[Cliente],[Categoria])
                    VALUES (@TituloSEO,@DescripcionSEO,@RutaImagen,@FechaActualizacion,@FechaVencimiento,@FechaBaja,
                    @Cliente,@Categoria)
                    SELECT SCOPE_IDENTITY();", avisos);

            avisos.Id = id; 
        }

        public async Task<IEnumerable<Avisos>> Listar(int usuarioId)
        {
            var connection = new SqlConnection (connectionstring);

            return await connection.QueryAsync<Avisos>(@"SELECT av.[Id],[TituloSEO],DescripcionSEO,RutaImagen,FechaActualizacion,
                                                        [FechaVencimiento],FechaBaja,av.[Cliente],cl.Empresa as NombreCliente,
                                                        ca.Categoria NombreCategoria
                                                        FROM [Magazine].[dbo].[Avisos] av	
                                                        INNER JOIN Clientes cl ON av.Cliente =cl.Id
                                                        INNER JOIN Categorias ca ON av.Categoria=ca.Id 
                                                        INNER JOIN RelClientesUsuarios relu ON relu.Cliente = cl.Id
                                                        WHERE relu.Usuario = @usuarioId
                                                        ORDER BY FechaActualizacion,TituloSEO", new { usuarioId });
        }
        public async Task<Avisos> AvisosPorId(int id, int usuarioId) 
        {
            var connection = new SqlConnection(connectionstring);
            return await connection.QueryFirstOrDefaultAsync<Avisos>(@"SELECT av.[Id]
                                    ,[TituloSEO], [DescripcionSEO],[RutaImagen],[FechaActualizacion]
                                    ,[FechaVencimiento],[FechaBaja],av.[Cliente],av.Categoria
                                    ,cl.Empresa as NombreCliente,ca.Categoria NombreCategoria 
                                    FROM [Magazine].[dbo].[Avisos] av	
                                    INNER JOIN Clientes cl ON av.Cliente =cl.Id
                                    INNER JOIN Categorias ca ON av.Categoria=ca.Id
									INNER JOIN RelClientesUsuarios RelU ON cl.Id=RelU.Cliente 
                                    WHERE av.id=@id
									AND RElu.Usuario=@usuarioId
                                    ORDER BY FechaActualizacion,TituloSEO", new {id,usuarioId});

        }
        public async Task<Avisos> ExisteAvisoxUsuarioxCliente(int usuarioid,int clienteid)
        {
            var connection = new SqlConnection(connectionstring);
            return await connection.QueryFirstOrDefaultAsync<Avisos>(@"SELECT 1 
                                    FROM [Magazine].[dbo].[Avisos] av	
                                    INNER JOIN Clientes cl ON av.Cliente =cl.Id
                                    INNER JOIN RelClientesUsuarios rel ON cl.Id=rel.cliente
                                    WHERE rel.usuario=@usuarioid AND cl.Id=@clienteid", new { usuarioid,clienteid });

        }
        public async Task Actualizar(Avisos avisos)
        {
            var connection = new SqlConnection(connectionstring);
            await connection.ExecuteAsync(@"UPDATE [dbo].[Avisos] SET [TituloSEO] = @TituloSEO
                ,[DescripcionSEO] = @DescripcionSEO,[RutaImagen] = @RutaImagen,[FechaActualizacion] = @FechaActualizacion
                ,[FechaVencimiento] = @FechaVencimiento,[FechaBaja] = @FechaBaja
                ,[Categoria] = @Categoria
                WHERE Id=@id", avisos);
        }
        public async Task Borrar(Avisos avisos)
        {
            var connection = new SqlConnection(connectionstring);
            await connection.ExecuteAsync(@"DELETE FROM [dbo].[Avisos] WHERE Id=@id", avisos);
            
        }
    }

    
}
