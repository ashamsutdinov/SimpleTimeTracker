using System;
using System.ComponentModel.Design;
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

        protected IDataReader ExecuteReader(string commandText, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command => command.ExecuteReader(), parameters);
        }

        protected IDataReader ExecuteReader(string commandText, out IDbCommand outCommand, params IDataParameter[] parameters)
        {
            return ExecuteCommand(commandText, command => command.ExecuteReader(), out outCommand, parameters);
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
