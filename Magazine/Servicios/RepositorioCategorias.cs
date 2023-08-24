using Dapper;
using Magazine.Models;
using Microsoft.Data.SqlClient;

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
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categorias>(@"SELECT Id,Categoria FROM Categorias");


        }
    }

    
}
