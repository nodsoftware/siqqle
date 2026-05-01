using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class ExecuteAsyncExtensions
{
    /// <summary>
    /// Execute parameterized SQL asynchronously.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>The number of rows affected.</returns>
    public static Task<int> ExecuteAsync<TStatement>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<TStatement> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
        where TStatement : SqlStatement
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.ExecuteAsync(commandText, parameters, transaction, commandTimeout, commandType);
    }

    /// <summary>
    /// Execute parameterized SQL asynchronously.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>The number of rows affected.</returns>
    public static Task<int> ExecuteAsync<TStatement>(
        this IDbConnection cnn,
        TStatement sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
        where TStatement : SqlStatement
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.ExecuteAsync(commandText, parameters, transaction, commandTimeout, commandType);
    }
}
