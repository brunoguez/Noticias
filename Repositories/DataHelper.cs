using Microsoft.Data.Sqlite;
using Shop;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Reflection;

namespace Noticias.Repositories
{
    public class DataHelper
    {
        private string connectionString;
        private DbProviderFactory providerFactory;

        public DataHelper()
        {
            connectionString = Settings.ConnectionString;
            providerFactory = SqliteFactory.Instance;
        }

        public int ExecuteNonQuery(string sql, SqliteParameter[]? sqlParameters = null)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(sqlParameters);
                    return command.ExecuteNonQuery();
                }
            }
            //using (SqlConnection connection = new(connectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand cmd = new(sql, connection))
            //    {
            //        if (sqlParameters != null)
            //        {
            //            foreach (SqlParameter item in sqlParameters)
            //            {
            //                if (item.Value == null) item.Value = DBNull.Value;
            //            }
            //            cmd.Parameters.AddRange(sqlParameters);
            //        }
            //        return cmd.ExecuteNonQuery();
            //    }
            //}
        }

        public object ExecuteScalar(string sql)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    return command.ExecuteScalar();
                }
            }
            //using (SqlConnection connection = new(connectionString))
            //{
            //    connection.Open();
            //    using (SqlCommand cmd = new(sql, connection))
            //    {
            //        return cmd.ExecuteScalar();
            //    }
            //}

        }

        public DbDataReader ExecuteReader(string sql)
        {
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            var command = connection.CreateCommand();
            command.CommandText = sql;
            connection.Open();
            return command.ExecuteReader();

            //SqlConnection connection = new(connectionString);
            //SqlCommand cmd = new(sql, connection);
            //return cmd.ExecuteReader();
        }

        public List<T> GetList<T>(string query)
        {
            try
            {
                List<T> listResponse = new();
                using (var reader = ExecuteReader(query))
                {
                    Type modelType = typeof(T);
                    PropertyInfo[] properties = modelType.GetProperties();

                    while (reader.Read())
                    {
                        var newListItem = Activator.CreateInstance<T>();

                        foreach (PropertyInfo property in properties)
                        {
                            ColumnAttribute? columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

                            if (columnAttribute != null)
                            {
                                string columnName = columnAttribute.Name;
                                int ordinal = reader.GetOrdinal(columnName);

                                if (reader.IsDBNull(ordinal)) { property.SetValue(newListItem, null); continue; }

                                object value = reader.GetValue(ordinal);
                                Type propertyType = property.PropertyType;

                                object convertedValue = Convert.ChangeType(value, propertyType);

                                property.SetValue(newListItem, convertedValue);
                            }
                        }

                        listResponse.Add(newListItem);
                    }
                }
                return listResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpandoObject> GetListDynamic(string query)
        {
            try
            {
                List<ExpandoObject> listResponse = new();
                using (var reader = ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        dynamic expando = new ExpandoObject();
                        foreach (DataRow row in reader.GetSchemaTable().Rows)
                        {
                            ((IDictionary<string, object>)expando)[row["ColumnName"].ToString()] = reader[row["ColumnName"].ToString()] == DBNull.Value ? null : reader[row["ColumnName"].ToString()];
                        }
                        listResponse.Add(expando);
                    }
                }
                return listResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
