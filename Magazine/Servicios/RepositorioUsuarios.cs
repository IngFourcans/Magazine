using Dapper;
using Magazine.Models;
using MySql.Data.MySqlClient;

namespace Magazine.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task ActualizarRolUsuario(int id, int rol);
        Task<Usuarios> BuscarUsuarioxEmail(string emailnormalizado);
        Task<int> CrearUsuario(Usuarios usuarios);
        Task<int> ObtenerIdRol(string nombreRol);
        Task<int> RolAsignado(int id, int rol);
    }
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
       

        public async Task<int> CrearUsuario(Usuarios usuarios)
        {
            using var connection = new MySqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Usuarios (Nombre, Usuario, TipoUsuario,
                                                    Email, EmailNormalizado, PasswordHash, EmailConfirmado)
                                                    VALUES (@Email, @Email, 1,
                                                    @Email, @EmailNormalizado, @PasswordHash, 1);
                                                    SELECT LAST_INSERT_ID();", usuarios);
            return id;
        }

        public async Task<Usuarios> BuscarUsuarioxEmail(string emailnormalizado)
        {
            using var connection = new MySqlConnection(connectionString);

            var usuario = await connection.QueryFirstOrDefaultAsync<Usuarios>(@"SELECT Id, Nombre, Usuario,
                                        TipoUsuario, Email, EmailNormalizado, PasswordHash, EmailConfirmado
                                        FROM Usuarios WHERE EmailNormalizado = @emailnormalizado",
                                            new { emailnormalizado });
            return usuario;
        }

        public async Task<int> ObtenerIdRol(string nombreRol)
        {
            using var connection = new MySqlConnection(connectionString);

            var id = await connection.QuerySingleOrDefaultAsync<int>(@"SELECT Id FROM TiposDeUsuarios WHERE NombreTipo = @nombreRol",
                                            new { nombreRol });
            return id;
        }

        public async Task ActualizarRolUsuario(int id, int rol)
        {
            using var connection = new MySqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Usuarios SET TipoUsuario = @rol WHERE Id = @id",
                                            new { rol, id });
        }

        public async Task<int> RolAsignado(int id, int rol)
        {
            using var connection = new MySqlConnection(connectionString);

            return await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) FROM Usuarios WHERE Id = @id AND TipoUsuario = @rol",
                                            new { rol, id });
        }

    




}

}   
