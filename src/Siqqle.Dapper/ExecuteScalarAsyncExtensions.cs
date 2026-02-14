using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class ExecuteScalarAsyncExtensions
{
    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        SqlSelect sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlUnion> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlBatch> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync(sql?.Go(), param, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        SqlUnion sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell selected as <see cref="object"/>.</returns>
    public static Task<object> ExecuteScalarAsync(
        this IDbConnection cnn,
        SqlBatch sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync<T>(
            sql?.Go(),
            param,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        SqlSelect sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync<T>(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlUnion> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync<T>(
            sql?.Go(),
            param,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlBatch> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        return cnn.ExecuteScalarAsync<T>(
            sql?.Go(),
            param,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        SqlUnion sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync<T>(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="cnn">The connection to execute on.</param>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    public static Task<T> ExecuteScalarAsync<T>(
        this IDbConnection cnn,
        SqlBatch sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return cnn.ExecuteScalarAsync<T>(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }
}
