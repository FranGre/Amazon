using Amazon.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Contexto
{
    internal class AppDbContexto : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }


        private static DbConnection CreateConnection()
        {
            var connection = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["AppConexion"].ConnectionString;

            return connection;
        }
    }
}
