using Dapper;
using Magazine.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Magazine.Servicios
{
    public interface IRepositorioSucursales
    {
        Task Borrar(int Id);
        Task Crear(Sucursal sucursal);
        Task Editar(SucursalCreacionViewModel sucursal);
        Task<IEnumerable<Sucursal>> Listar(int cliente, int usuario);
        Task<Sucursal> ObtenerSucursalxId(int sucursal, int usuario);
    }

    public class RepositorioSucursales: IRepositorioSucursales
    {
        private readonly string connectionString;


        public RepositorioSucursales(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task Crear(Sucursal sucursal)
        {
            using var connection = new MySqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Sucursales
            (NombreSucursal, SucursalCentral, Calle, Nro, Piso, Departamento, InformacionAdicional,
             Telefono, Whatsapp, Provincia, Localidad, Cliente)
            VALUES
            (@NombreSucursal, @SucursalCentral, @Calle, @Nro, @Piso, @Departamento, @InformacionAdicional,
             @Telefono, @Whatsapp, @Provincia, @Localidad, @Cliente);
            SELECT LAST_INSERT_ID();", sucursal);
            sucursal.Id = id;
        }

        public async Task Editar(SucursalCreacionViewModel sucursal)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.QueryAsync(@"UPDATE Sucursales
            SET NombreSucursal = @NombreSucursal, SucursalCentral = @SucursalCentral, Calle = @Calle,
            Nro = @Nro, Piso = @Piso, Departamento = @Departamento, InformacionAdicional = @InformacionAdicional,
            Telefono = @Telefono, Whatsapp = @Whatsapp, Provincia = @Provincia, Localidad = @Localidad
            WHERE Id = @Id", sucursal);
        }

        public async Task Borrar(int Id)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.QueryAsync(@"DELETE FROM Sucursales WHERE Id = @Id", new { Id });
        }

        public async Task<Sucursal> ObtenerSucursalxId(int sucursalid, int usuario)
        {
            using var connection = new MySqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Sucursal>(@"SELECT s.Id, NombreSucursal, SucursalCentral, Calle,
            Nro, Piso, Departamento, InformacionAdicional, Telefono, Whatsapp, s.Provincia, s.Localidad,
            s.Cliente, c.Empresa as ClienteNombre, p.Provincia as ProvinciaNombre,
            l.Localidad as LocalidadNombre
            FROM Sucursales s
            INNER JOIN Clientes c ON s.Cliente = c.Id
            INNER JOIN Provincias p ON s.Provincia = p.Id
            INNER JOIN Localidades l ON s.Localidad = l.Id
            INNER JOIN RelClientesUsuarios relu ON c.Id = relu.Cliente
            WHERE relu.Usuario = @usuario AND s.Id = @sucursalid", new { usuario, sucursalid });
        }

        public async Task<IEnumerable<Sucursal>> Listar(int cliente, int usuario)
        {
            using var con = new MySqlConnection(connectionString);
            return await con.QueryAsync<Sucursal>(@"SELECT s.Id, NombreSucursal, SucursalCentral, Calle, Nro,
            Piso, Departamento, InformacionAdicional, Telefono, Whatsapp, s.Provincia, s.Localidad,
            s.Cliente, c.Empresa as ClienteNombre, p.Provincia as ProvinciaNombre,
            l.Localidad as LocalidadNombre
            FROM Sucursales s
            INNER JOIN Clientes c ON s.Cliente = c.Id
            INNER JOIN Provincias p ON s.Provincia = p.Id
            INNER JOIN Localidades l ON s.Localidad = l.Id
            INNER JOIN RelClientesUsuarios relu ON c.Id = relu.Cliente
            WHERE s.Cliente = @cliente AND relu.Usuario = @usuario", new { cliente, usuario });
        }

    }


}
