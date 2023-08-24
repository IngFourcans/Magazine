using Dapper;
using Magazine.Models;
using Microsoft.Data.SqlClient;

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
            using var connection = new SqlConnection(connectionString);
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
