//using Microsoft.Data.SqlClient;
//using System.Data;

//public class DatabaseHelper
//{
//    private readonly string _connectionString;

//    public DatabaseHelper(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public IDbConnection CreateConnection()
//    {
//        return new SqlConnection(_connectionString);
//    }

//    public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
//    {
//        return new SqlCommand(commandText, (SqlConnection)connection)
//        {
//            CommandType = commandType
//        };
//    }

//    public IDbDataParameter CreateParameter(IDbCommand command, string parameterName, object parameterValue)
//    {
//        IDbDataParameter parameter = command.CreateParameter();
//        parameter.ParameterName = parameterName;
//        parameter.Value = parameterValue;
//        return parameter;
//    }
//}
