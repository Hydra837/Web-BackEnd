using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
	public sealed class Connection
	{
		private string _connectionString;


		public Connection(string connectionString)
		{
			_connectionString = connectionString;

			using (SqlConnection connection = CreateConnection())
			{
				connection.Open();
			}
		}
		public SqlConnection CreateConnection()
		{
			SqlConnection connection = new SqlConnection();
			connection.ConnectionString = _connectionString;

			return connection;
		}
        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
