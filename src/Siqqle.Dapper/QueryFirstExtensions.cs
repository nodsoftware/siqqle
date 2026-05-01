using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class QueryFirstExtensions
{
    /// <summary>
    /// Return a dynamic object with properties matching the columns.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static dynamic QueryFirst(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryFirst(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Return a dynamic object with properties matching the columns.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static dynamic QueryFirst(
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
        return cnn.QueryFirst(commandText, parameters, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Return a dynamic object with properties matching the columns.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static dynamic QueryFirstOrDefault(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryFirstOrDefault(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Return a dynamic object with properties matching the columns.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    public static dynamic QueryFirstOrDefault(
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
        return cnn.QueryFirstOrDefault(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
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
    public static T QueryFirst<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryFirst<T>(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
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
    public static T QueryFirst<T>(
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
        return cnn.QueryFirst<T>(commandText, parameters, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
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
    public static T QueryFirstOrDefault<T>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.QueryFirstOrDefault<T>(
            sql?.Go(),
            param,
            transaction,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
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
    public static T QueryFirstOrDefault<T>(
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
        return cnn.QueryFirstOrDefault<T>(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType
        );
    }
}
