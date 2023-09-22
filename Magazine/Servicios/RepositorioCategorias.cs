using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;

namespace Magazine.Servicios
{

    public interface IRepositorioCategorias
    {
        Task<IEnumerable<Categorias>> Listar();
    }

    public class RepositorioCategorias:IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<Categorias>> Listar()
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryAsync<Categorias>(@"SELECT Id,Categoria FROM Categorias");


        }
    }

    
}
