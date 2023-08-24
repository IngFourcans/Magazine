using Dapper;
using Magazine.Models;
using Microsoft.Data.SqlClient;

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
        private readonly string connectionstring;

        public RepositorioRubros(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Rubros rubros) 
        {
            using var connection= new SqlConnection(connectionstring);
            var id = await connection.QuerySingleAsync<int>("RubrosInsetar",
                                                            new {Rubro=rubros.Rubro},
                                                            commandType: System.Data.CommandType.StoredProcedure);
            rubros.Id = id;
        }

        public async Task<bool> Existe(string rubro)
        {

            using var connection = new SqlConnection(connectionstring);

            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 from Rubros
                WHERE rubro=@Rubro;", new { rubro });
            return existe == 1;

        }
        public async Task<IEnumerable<Rubros>> Listar() 
        {
            using var connection = new SqlConnection(connectionstring);

            return await connection.QueryAsync<Rubros>(@"SELECT Id, [Rubro], Orden FROM [Magazine].[dbo].[Rubros] ORDER BY Orden");
            
        }
        public async Task Actualizar(Rubros rubros) 
        {
            using var connection =new SqlConnection(connectionstring);

            await connection.ExecuteAsync(@"UPDATE Rubros SET Rubro=@Rubro WHERE Id=@Id",rubros);
        }
        public async Task Borrar(Rubros rubros)
        {
            using var connection = new SqlConnection(connectionstring);

            await connection.ExecuteAsync(@"DELETE FROM RelClientesRubros WHERE IdRubro=@Id
                                            DELETE Rubros WHERE Id=@Id", rubros);
        }

        public async Task<Rubros> RubroXId(int id)
        {
            using var connection=new SqlConnection(connectionstring);
            return await connection.QueryFirstOrDefaultAsync<Rubros>(@"SELECT [Id]
                                                                    ,[Rubro],[Orden]
                                                                    FROM [Magazine].[dbo].[Rubros]
                                                                    WHERE id=@Id", new {id});
        }
        public async Task Ordenar(IEnumerable<Rubros> rubrosOrdenados)
        {
            var query = "UPDATE Rubros SET Orden=@Orden WHERE Id=@Id";
            using var connection= new SqlConnection(connectionstring);
            await connection.ExecuteAsync(query,rubrosOrdenados);



        }
    }

}
