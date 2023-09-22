using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;
using System.Data;

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
            using var connection = new MySqlConnection(connectionString);

            return await connection.QueryAsync<Clientes>(@"SELECT c.Id, Empresa, Email, ReferenteNombre, CelularReferente,
                                                        IFNULL(a.Id, -1) AS aviso
                                                    FROM Magazine.Clientes c
                                                    INNER JOIN RelClientesUsuarios u ON c.Id = u.cliente
                                                    LEFT JOIN Avisos a ON c.Id = a.cliente
                                                    WHERE u.usuario = @usuarioId
                                                    ORDER BY Empresa", new { usuarioId });
        }

        public async Task<IEnumerable<Clientes>> Listar(int cliente, int usuarioId)
        {
            using var connection = new MySqlConnection(connectionString);

            return await connection.QueryAsync<Clientes>(@"SELECT c.Id, Empresa, Email, ReferenteNombre, CelularReferente, -1 AS aviso
                                                    FROM Magazine.Clientes c
                                                   INNER JOIN RelClientesUsuarios u on c.Id = u.cliente
                                                   WHERE u.usuario = @usuarioId AND c.Id = @cliente
                                                    ORDER BY Empresa", new { usuarioId, cliente });
        }

        public async Task<Clientes> ClienteXId(int id, int usuarioId)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Clientes>(@"SELECT Id, Empresa, Email, Instagram
                                                    , Web, Facebook, Linkedin, Twitter
                                                    , CUIT, RazonSocial, DomicilioLegal, ReferenteNombre
                                                    , CelularReferente 
                                                    FROM Magazine.Clientes c
                                                    INNER JOIN RelClientesUsuarios u on c.Id = u.cliente
                                                    WHERE u.usuario = @usuarioId
                                                    AND c.Id = @id
                                                    ORDER BY Empresa", new { id, usuarioId });
        }

        public async Task Crear(Clientes clientes)
        {
            using var connection = new MySqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Clientes
            (Empresa, Email, Instagram, Web, Facebook, Linkedin, Twitter
            , CUIT, RazonSocial, DomicilioLegal, ReferenteNombre, CelularReferente)
            VALUES (@Empresa, @Email, @Instagram, @Web, @Facebook, @Linkedin, @Twitter
            , @CUIT, @RazonSocial, @DomicilioLegal, @ReferenteNombre, @CelularReferente);
            SELECT LAST_INSERT_ID();", clientes);

            clientes.Id = id;
        }

        public async Task Actualizar(Clientes cliente)
        {
            var connection = new MySqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Clientes
            SET Empresa = @Empresa, Email = @Email, Instagram = @Instagram, Web = @Web
            , Facebook = @Facebook, Linkedin = @Linkedin, Twitter = @Twitter, CUIT = @CUIT
            , RazonSocial = @RazonSocial, DomicilioLegal = @DomicilioLegal
            , ReferenteNombre = @ReferenteNombre, CelularReferente = @CelularReferente
            WHERE Id = @Id", cliente);
        }

        public async Task Borrar(int id)
        {
            var connection = new MySqlConnection(connectionString);
            await connection.ExecuteAsync("ClientesBorrar", new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RubrosSeleccionados>> ObtenerRubrosxCliente(int id)
        {
            using var connection = new MySqlConnection(connectionString);

            return await connection.QueryAsync<RubrosSeleccionados>(@"SELECT r.id, Rubro, Orden, 
                            CASE WHEN cr.IdCliente IS NOT NULL THEN 'true' ELSE 'false' END seleccionado 
                            FROM Rubros r
                            LEFT JOIN RelClientesRubros cr ON r.Id = cr.IdRubro AND cr.IdCliente = @id
                            ORDER BY r.rubro", new { id });
        }

        public async Task ActualizarRubrosSeleccionados(List<int> rubrosSeleccionados, int cliente)
        {
            var connection = new MySqlConnection(connectionString);
            var rs = string.Join(",", rubrosSeleccionados);
            await connection.ExecuteAsync("ActualizarRubrosSeleccionados",
                new { rubrosSeleccionados = rs, cliente = cliente },
                commandType: CommandType.StoredProcedure);
        }

    }
}
