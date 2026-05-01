using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class QueryAsyncExtensions
{
    /// <summary>
    /// Return a sequence of dynamic objects with properties matching the columns asynchronously.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static Task<IEnumerable<dynamic>> QueryAsync(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryAsync(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Return a sequence of dynamic objects with properties matching the columns asynchronously.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static Task<IEnumerable<dynamic>> QueryAsync(
        this IDbConnection cnn,
        SqlSelect sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.QueryAsync(commandText, parameters, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Executes a query asynchronously, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>
    /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
    /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryAsync<T>(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Executes a query asynchronously, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>
    /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
    /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDbConnection cnn,
        SqlSelect sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.QueryAsync<T>(commandText, parameters, transaction, commandTimeout, commandType);
    }
}
