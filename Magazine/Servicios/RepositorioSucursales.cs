using Dapper;
using Magazine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
        private readonly string connectionstring;


        public RepositorioSucursales(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");

        }
        
        public async Task Crear(Sucursal sucursal) 
        {
            using var connection = new SqlConnection(connectionstring);
            var id = await connection.QuerySingleAsync<int> (@"INSERT INTO [dbo].[Sucursales]
                                                            ([NombreSucursal],[SucursalCentral],[Calle]
                                                            ,[Nro],[Piso],[Departamento],[InformacionAdicional]
                                                            ,[Telefono],[Whatsapp],[Provincia],[Localidad],[Cliente])
                                                            VALUES
                                                            (@NombreSucursal,@SucursalCentral,@Calle,@Nro
                                                            ,@Piso,@Departamento,@InformacionAdicional,@Telefono
                                                            ,@Whatsapp,@Provincia,@Localidad,@Cliente)
                                                            SELECT SCOPE_IDENTITY();", sucursal);
            sucursal.Id = id;
        }
        public async Task Editar(SucursalCreacionViewModel sucursal)
        {
            using var connection=new SqlConnection(connectionstring);
            await connection.QueryAsync(@"UPDATE [dbo].[Sucursales]
                            SET [NombreSucursal] = @NombreSucursal
                            ,[SucursalCentral] = @SucursalCentral,[Calle] = @Calle,[Nro] = @Nro,[Piso] = @Piso
                            ,[Departamento] = @Departamento,[InformacionAdicional] = @InformacionAdicional
                            ,[Telefono] = @Telefono,[Whatsapp] = @Whatsapp,[Provincia] = @Provincia,[Localidad] = @Localidad
                            WHERE Id=@Id",sucursal);
            
        }
        public async Task Borrar(int Id)
        {
            using var connection = new SqlConnection(connectionstring);
            await connection.QueryAsync(@"DELETE FROM [dbo].[Sucursales] WHERE Id=@Id", new {Id});

        }

        public async Task<Sucursal> ObtenerSucursalxId(int sucursalid, int usuario)
        {
            using var connection=new SqlConnection(connectionstring);
            return await connection.QueryFirstOrDefaultAsync<Sucursal>(@"SELECT s.Id,NombreSucursal,SucursalCentral,Calle
                                    ,Nro,Piso,Departamento,InformacionAdicional
                                    ,Telefono,Whatsapp,s.Provincia,s.Localidad
                                    ,s.Cliente,c.Empresa ClienteNombre
                                    ,p.Provincia as ProvinciaNombre, l.Localidad as LocalidadNombre
                                    FROM Magazine.dbo.Sucursales s
                                    INNER JOIN Clientes c ON s.Cliente=c.Id
                                    INNER JOIN Provincias p ON s.Provincia=p.Id
                                    INNER JOIN Localidades l ON s.Localidad=l.Id
									INNER JOIN RelClientesUsuarios relu on c.Id=relu.Cliente
                                    WHERE relu.Usuario=@usuario AND s.Id=@sucursalid", new {  usuario, sucursalid });
        }

        public async Task<IEnumerable<Sucursal>> Listar(int cliente,int usuario)
        {
            using var con = new SqlConnection(connectionstring);
            return await con.QueryAsync<Sucursal>(@"SELECT s.[Id],[NombreSucursal],[SucursalCentral],[Calle]
                                                                ,[Nro],[Piso],[Departamento],[InformacionAdicional]
                                                                ,[Telefono],[Whatsapp],s.[Provincia],s.[Localidad]
                                                                ,s.[Cliente],c.Empresa ClienteNombre
                                                                ,p.Provincia as ProvinciaNombre, l.Localidad as LocalidadNombre
                                                                  FROM [Magazine].[dbo].[Sucursales] s
                                                                  INNER JOIN Clientes c ON s.Cliente=c.Id
                                                                  INNER JOIN Provincias p ON s.Provincia=p.Id
                                                                  INNER JOIN Localidades l ON s.Localidad=l.Id
                                                                  INNER JOIN RelClientesUsuarios relu on c.Id=relu.Cliente
																  WHERE s.Cliente=@cliente and  relu.Usuario=@usuario", new { cliente,usuario });
        }
    }

    
}
