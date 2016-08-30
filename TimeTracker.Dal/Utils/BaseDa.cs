using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TimeTracker.Dal.Utils
{
    public abstract class BaseDa
    {
        private readonly string _connectionString;

        protected BaseDa()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["main"].ConnectionString;
        }

        protected TResult ExecuteScalar<TResult>(string commandText, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command =>
            {
                var result = command.ExecuteScalar();
                return (TResult)result;
            }, parameters);
        }

        protected int ExecuteNonQuery(string commandText, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command => command.ExecuteNonQuery(), parameters);
        }

        protected TResult ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> readerAction, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command =>
            {
                var reader = command.ExecuteReader();
                return readerAction(reader);
            }, parameters);
        }

        protected TResult ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> readerAction,  out IDbCommand outCommand, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command =>
            {
                var reader = command.ExecuteReader();
                return readerAction(reader);
            }, out outCommand, parameters);
        }

        protected IDbDataParameter CreateParameter(string name, SqlDbType type, object value)
        {
            var parameter = new SqlParameter(name, type) { Value = value ?? DBNull.Value };
            return parameter;
        }

        protected IDbDataParameter CreateKeyCollectionParameter(string name, IEnumerable<string> values)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("[Id]");
            foreach (var value in values)
            {
                dataTable.Rows.Add(value);
            }
            var parameter = new SqlParameter(name, SqlDbType.Structured)
            {
                TypeName = "[dbo].[KeyCollection]",
                Value = dataTable
            };
            return parameter;
        }

        protected IDbDataParameter CreateKeyValueCollectionParameter(string name, IEnumerable<KeyValuePair<string, string>> values)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("[Id]");
            dataTable.Columns.Add("[Value]");
            foreach (var value in values)
            {
                var row = dataTable.NewRow();
                row["[Id]"] = value.Key;
                row["[Value]"] = value.Value;
                dataTable.Rows.Add(row);
            }
            var parameter = new SqlParameter(name, SqlDbType.Structured)
            {
                TypeName = "[dbo].[KeyValueCollection]",
                Value = dataTable
            };
            return parameter;
        }

        protected List<TEntity> Read<TEntity>(IDataReader reader, Func<IDataRecord, TEntity> read)
        {
            var result = new List<TEntity>();
            while (reader.Read())
            {
                result.Add(read(reader));
            }
            return result;
        }

        protected TEntity ReadSingle<TEntity>(IDataReader reader, Func<IDataRecord, TEntity> read)
        {
            return reader.Read() ? read(reader) : default(TEntity);
        }

        protected TValue GetOutputValue<TValue>(IDbCommand command, string key)
        {
            var parameter = command.Parameters["key"] as SqlParameter;
            if (parameter != null)
            {
                return (TValue)parameter.Value;
            }
            return default(TValue);
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private static IDbCommand CreateCommand(IDbConnection connection, string text)
        {
            var command = connection.CreateCommand();
            command.CommandText = text;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        private TResult ExecuteCommand<TResult>(string commandText, Func<IDbCommand, TResult> action, params IDataParameter[] parameters)
        {
            IDbCommand outCommand;
            return ExecuteCommand(commandText, action, out outCommand, parameters);
        }

        private TResult ExecuteCommand<TResult>(string commandText, Func<IDbCommand, TResult> action, out IDbCommand outCommand, params IDataParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    var command = CreateCommand(connection, commandText);
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    var result = action(command);
                    outCommand = command;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = color;
                outCommand = null;
                return default(TResult);
            }
        }
    }
}
