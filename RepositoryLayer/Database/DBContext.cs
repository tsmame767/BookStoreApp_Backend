using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Database
{
    public class DBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string ConnectStr;

        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            this.ConnectStr = _configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(this.ConnectStr);
    }
}
