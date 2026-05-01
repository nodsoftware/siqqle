using System.Data;
using System.Threading;
using Dapper;
using Siqqle.Expressions;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

internal class CommandDefinitionFactory
{
    public static CommandDefinition Create<TStatement>(
        ISqlSyntaxEnd<TStatement> sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default
    )
        where TStatement : SqlStatement
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return new CommandDefinition(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType,
            flags,
            cancellationToken
        );
    }

    public static CommandDefinition Create<TStatement>(
        TStatement sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags flags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default
    )
        where TStatement : SqlStatement
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param);
        return new CommandDefinition(
            commandText,
            parameters,
            transaction,
            commandTimeout,
            commandType,
            flags,
            cancellationToken
        );
    }
}
