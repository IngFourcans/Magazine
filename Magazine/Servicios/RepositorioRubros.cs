using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace Magazine.Servicios
{

    public interface IRepositorioRubros
    {
        Task Actualizar(Rubros rubros);
        Task Borrar(Rubros rubros);
        Task Crear(Rubros rubros);
        Task<bool> Existe(string nombrerubro);
        Task<IEnumerable<Rubros>> Listar();
        Task Ordenar(IEnumerable<Rubros> rubrosOrdenados);
        Task<Rubros> RubroXId(int id);
    }
    public class RepositorioRubros: IRepositorioRubros
    {
        private readonly string connectionString;

        public RepositorioRubros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Rubros rubros)
        {
            using var connection = new MySqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("RubrosInsetar",
                                                            new { Rubro = rubros.Rubro },
                                                            commandType: CommandType.StoredProcedure);
            rubros.Id = id;
        }

        public async Task<bool> Existe(string rubro)
        {
            using var connection = new MySqlConnection(connectionString);

            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 from Rubros
        WHERE Rubro = @rubro;", new { rubro });
            return existe == 1;
        }

        public async Task<IEnumerable<Rubros>> Listar()
        {
            using var connection = new MySqlConnection(connectionString);

            return await connection.QueryAsync<Rubros>(@"SELECT Id, Rubro, Orden FROM Rubros ORDER BY Orden");
        }

        public async Task Actualizar(Rubros rubros)
        {
            using var connection = new MySqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Rubros SET Rubro = @Rubro WHERE Id = @Id", rubros);
        }

        public async Task Borrar(Rubros rubros)
        {
            using var connection = new MySqlConnection(connectionString);

            await connection.ExecuteAsync(@"DELETE FROM RelClientesRubros WHERE IdRubro = @Id;
                                    DELETE FROM Rubros WHERE Id = @Id;", rubros);
        }

        public async Task<Rubros> RubroXId(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Rubros>(@"SELECT Id, Rubro, Orden FROM Rubros WHERE Id = @id", new { id });
        }

        public async Task Ordenar(IEnumerable<Rubros> rubrosOrdenados)
        {
            var query = "UPDATE Rubros SET Orden = @Orden WHERE Id = @Id";
            using var connection = new MySqlConnection(connectionString);
            await connection.ExecuteAsync(query, rubrosOrdenados);
        }

    }

}
