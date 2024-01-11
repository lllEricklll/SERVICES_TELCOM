using BANCO_AMARILLO.Modelos;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BANCO_AMARILLO.Conexion
{   
    public class ConsultasDapper
    {
        private readonly IDbConnection db;
        private readonly string connectionString;

        public ConsultasDapper(string connectionString)
        {
            this.connectionString = connectionString;
            db = new SqlConnection(connectionString);
        }
        public IEnumerable<InformacionRed> BusquedaGeneral()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<InformacionRed>("BuscarGeneralRed", commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InformacionExcel> BusquedaGeneralExcel()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<InformacionExcel>("BusquedaGeneralExcel", commandType: CommandType.StoredProcedure);
            }
        }


        public IEnumerable<InformacionRed> BusquedaPorIp(string ip)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IP", ip);

                return connection.Query<InformacionRed>("BusquedaPorIP", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<InformacionRed> BusquedaPorCB(string cb)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CB", cb);

                return connection.Query<InformacionRed>("BusquedaPorCB", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InformacionRed> BusquedaPorNumeroSerie(string sre)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Num_Serie", sre);

                return connection.Query<InformacionRed>("BusquedaPorSerie", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public IEnumerable<InformacionRed>  BusquedaPorMac(string mac)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@MAC", mac);

                return connection.Query<InformacionRed>("BusquedaPorMac", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InformacionRed> BusquedaPorId(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);

                return connection.Query<InformacionRed>("BusquedaPorID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int InsertarTransaccionRed(InformacionRed transaccion)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IP", transaccion.IP);
                parameters.Add("@MASCARA", transaccion.MASCARA);
                parameters.Add("@GATEWAY", transaccion.GATEWAY);
                parameters.Add("@SECTOR", transaccion.SECTOR);
                parameters.Add("@TIPO", transaccion.TIPO);
                parameters.Add("@NOMBRE", transaccion.NOMBRE);
                parameters.Add("@MAC", transaccion.MAC);
                parameters.Add("@Numero_de_serie", transaccion.Numero_de_serie);
                parameters.Add("@SWITCH", transaccion.SWITCH);
                parameters.Add("@PUERTO", transaccion.PUERTO);
                parameters.Add("@PTO_RED", transaccion.PTO_RED);
                parameters.Add("@USUARIO", transaccion.USUARIO);
                parameters.Add("@CONTRASENA", transaccion.CONTRASENA);
                parameters.Add("@CB", transaccion.CB);
                parameters.Add("@MARCA", transaccion.MARCA);
                parameters.Add("@MODELO", transaccion.MODELO);

                return connection.Execute("InsertarInformacionRed", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        public bool ValidarCredenciales(string nombre, string contrasena)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", nombre);
                parameters.Add("@Contrasena", contrasena);
                var result = connection.Query<string>("ValidarAccesoUsuario", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

                return result == "Acceso Correcto";
            }
        }

        public string GenerarTokenJwt(string usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("mi_clave_super_secreta"); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario)
                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /*public int RegistrarUsuario(Usuarios usuarios)
        {
            using (IDbConnection connection= new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NombreUsuario", usuarios.Nombre);
                parameters.Add("@Contrasena", usuarios.Contrasena);
                parameters.Add("@Apellido", usuarios.Apellido);
                parameters.Add("@Email", usuarios.Email);

                return connection.Execute("CrearUsuario", parameters, commandType: CommandType.StoredProcedure);
            }
        }*/
        public int ActualizarInformacionRed(int id, ActualizarRed transaccion)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);
                parameters.Add("@IP", transaccion.IP);
                parameters.Add("@MASCARA", transaccion.MASCARA);
                parameters.Add("@GATEWAY", transaccion.GATEWAY);
                parameters.Add("@SECTOR", transaccion.SECTOR);
                parameters.Add("@TIPO", transaccion.TIPO);
                parameters.Add("@NOMBRE", transaccion.NOMBRE);
                parameters.Add("@MAC", transaccion.MAC);
                parameters.Add("@Numero_de_serie", transaccion.Numero_de_serie);
                parameters.Add("@SWITCH", transaccion.SWITCH);
                parameters.Add("@PUERTO", transaccion.PUERTO);
                parameters.Add("@PTO_RED", transaccion.PTO_RED);
                parameters.Add("@USUARIO", transaccion.USUARIO);
                parameters.Add("@CONTRASENA", transaccion.CONTRASENA);
                parameters.Add("@CB", transaccion.CB);
                parameters.Add("@MARCA", transaccion.MARCA);
                parameters.Add("@MODELO", transaccion.MODELO);

                return connection.Execute("ActualizarInformacionRedPorID", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public void EliminarInformacionRedID (int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);

                connection.Execute("EliminarInformacionRedPorID", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void EliminarInformacionExcel (int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID_EXCEL", id);

                connection.Execute("DeleteInformacionExcel",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InformacionRed> EjecutarFunctionExcel(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);

                // Ejecutar el procedimiento almacenado
                var result = connection.Query<InformacionRed>("functionExcel", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public void EliminarCliente(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id_cliente", id);

                connection.Execute("EliminarCliente", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
