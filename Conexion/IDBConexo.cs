using System.Data;
using System.Data.SqlClient;

namespace BANCO_AMARILLO.Conexion
{
    public interface IDBConexto
    {
        IDbConnection Connection { get; }
    }

    public class DatabaseContext : IDBConexto
    {
        public IDbConnection Connection { get; }

        public DatabaseContext(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            Connection = new SqlConnection(connectionString);
        }
    }

}
