using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

using Dapper;

namespace DataAccessLibrary.SQLiteDataAccess
{
	internal static class SQLiteDataAccess
	{
		internal static List<T> ReadData<T, U>(string sqlStatement, U parameters, string connectionString)
		{
			using ( IDbConnection connection = new SQLiteConnection(connectionString) )
			{
				List<T> data = connection.Query<T>(sqlStatement, parameters).ToList();
				return data;
			}
		}

		internal static void WriteData<T>(string sqlStatement, T parameters, string connectionString)
		{
			using ( IDbConnection connection = new SQLiteConnection(connectionString) )
			{
				_ = connection.Execute(sqlStatement, parameters);
			}
		}
	}
}
