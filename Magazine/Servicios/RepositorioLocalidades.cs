using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;

namespace Magazine.Servicios
{
    public class RepositorioLocalidades:IRepositorioLocalidades
    {
        private readonly string connectionString;
        public RepositorioLocalidades(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<Localidades>> Listar(int Id)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryAsync<Localidades>(@"SELECT Id,Localidad FROM Localidades 
                                                            WHERE Provincia=@Id
                                                            ORDER BY Localidad", new { Id });


        }
    }

    public interface IRepositorioLocalidades
    {
        
        Task<IEnumerable<Localidades>> Listar(int Id);
    }
}
