using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class ExecuteReaderAsyncExtensions
{
    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlUnion> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteReaderAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlBatch> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteReaderAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        SqlUnion sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteReaderAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        SqlBatch sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteReaderAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteReaderAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously and return a <see cref="Task{IDataReader}"/>.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    public static Task<IDataReader> ExecuteReaderAsync(
        this IDbConnection cnn,
        SqlSelect sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteReaderAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }
}
