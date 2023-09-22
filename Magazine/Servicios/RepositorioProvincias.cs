using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;
namespace Magazine.Servicios
{

    public interface IRepositorioProvincias
    {
        Task<IEnumerable<Provincias>> Listar();
    }
    public class RepositorioProvincias: IRepositorioProvincias
    {
        private readonly string connectionstring; 
        public RepositorioProvincias(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Provincias>> Listar ()
        {
            using var connection = new MySqlConnection(connectionstring);
            return await connection.QueryAsync<Provincias>(@"SELECT Id, Provincia FROM Provincias 
                                                            ORDER BY Provincia");

        }

    }

}
