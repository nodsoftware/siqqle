using System.Data;
using System.Threading.Tasks;
using Dapper;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;
using static Dapper.SqlMapper;

namespace Siqqle.Dapper;

public static class QueryMultipleExtensions
{
    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    public static GridReader QueryMultiple<TStatement>(
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
        return cnn.QueryMultiple(sql?.Go(), param, transaction, commandTimeout, commandType, dialect);
    }

    /// <summary>
    /// Execute a command that returns multiple result sets, and access each in turn.
    /// </summary>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    public static GridReader QueryMultiple<TStatement>(
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
        return cnn.QueryMultiple(commandText, parameters, transaction, commandTimeout, commandType);
    }
}
