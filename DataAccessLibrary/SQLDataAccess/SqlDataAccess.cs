using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;

namespace DataAccessLibrary.SQLDataAccess
{
	internal static class SqlDataAccess
	{
		internal static List<T> ReadData<T, U>(string sqlStatement, U parameters, string connectionString)
		{
			using ( IDbConnection connection = new SqlConnection(connectionString) )
			{
				List<T> data = connection.Query<T>(sqlStatement, parameters).ToList();
				return data;
			}
		}

		internal static void WriteData<T>(string sqlStatement, T parameters, string connectionString)
		{
			using ( IDbConnection connection = new SqlConnection(connectionString) )
			{
				_ = connection.Execute(sqlStatement, parameters);
			}
		}
	}
}
