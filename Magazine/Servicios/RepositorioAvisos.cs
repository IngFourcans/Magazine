using Dapper;
using Magazine.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        private readonly string connectionString;

        public RepositorioAvisos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Avisos avisos)
        {
            using var connection = new MySqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Avisos (TituloSEO, DescripcionSEO,
            RutaImagen, FechaActualizacion, FechaVencimiento, FechaBaja, Cliente, Categoria)
            VALUES (@TituloSEO, @DescripcionSEO, @RutaImagen, @FechaActualizacion, @FechaVencimiento, @FechaBaja,
            @Cliente, @Categoria);
            SELECT LAST_INSERT_ID();", avisos);

            avisos.Id = id;
        }

        public async Task<IEnumerable<Avisos>> Listar(int usuarioId)
        {
            using var connection = new MySqlConnection(connectionString);

            return await connection.QueryAsync<Avisos>(@"SELECT av.Id, TituloSEO, DescripcionSEO, RutaImagen, FechaActualizacion,
                                                FechaVencimiento, FechaBaja, av.Cliente, cl.Empresa as NombreCliente,
                                                ca.Categoria as NombreCategoria
                                                FROM Avisos av	
                                                INNER JOIN Clientes cl ON av.Cliente = cl.Id
                                                INNER JOIN Categorias ca ON av.Categoria = ca.Id
                                                INNER JOIN RelClientesUsuarios relu ON relu.Cliente = cl.Id
                                                WHERE relu.Usuario = @usuarioId
                                                ORDER BY FechaActualizacion, TituloSEO", new { usuarioId });
        }

        public async Task<Avisos> AvisosPorId(int id, int usuarioId)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Avisos>(@"SELECT av.Id, TituloSEO, DescripcionSEO, RutaImagen, FechaActualizacion,
                            FechaVencimiento, FechaBaja, av.Cliente, av.Categoria,
                            cl.Empresa as NombreCliente, ca.Categoria as NombreCategoria 
                            FROM Avisos av	
                            INNER JOIN Clientes cl ON av.Cliente = cl.Id
                            INNER JOIN Categorias ca ON av.Categoria = ca.Id
                            INNER JOIN RelClientesUsuarios RelU ON cl.Id = RelU.Cliente 
                            WHERE av.Id = @id
                            AND RelU.Usuario = @usuarioId
                            ORDER BY FechaActualizacion, TituloSEO", new { id, usuarioId });
        }

        public async Task<Avisos> ExisteAvisoxUsuarioxCliente(int usuarioid, int clienteid)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Avisos>(@"SELECT 1 
                            FROM Avisos av	
                            INNER JOIN Clientes cl ON av.Cliente = cl.Id
                            INNER JOIN RelClientesUsuarios rel ON cl.Id = rel.Cliente
                            WHERE rel.Usuario = @usuarioid AND cl.Id = @clienteid", new { usuarioid, clienteid });
        }

        public async Task Actualizar(Avisos avisos)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Avisos SET TituloSEO = @TituloSEO,
            DescripcionSEO = @DescripcionSEO, RutaImagen = @RutaImagen, FechaActualizacion = @FechaActualizacion,
            FechaVencimiento = @FechaVencimiento, FechaBaja = @FechaBaja,
            Categoria = @Categoria
            WHERE Id = @id", avisos);
        }

        public async Task Borrar(Avisos avisos)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM Avisos WHERE Id = @id", avisos);
        }

    }


}
