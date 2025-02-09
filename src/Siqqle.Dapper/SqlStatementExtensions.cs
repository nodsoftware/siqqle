using System.Data;
using System.Threading;
using Dapper;
using Siqqle.Expressions;

namespace Siqqle.Dapper;

public static class SqlStatementExtensions
{
    public static CommandDefinition ToCommandDefinition<TStatement>(
        this TStatement statement,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null,
        CommandFlags commandFlags = CommandFlags.Buffered,
        CancellationToken cancellationToken = default
    )
        where TStatement : SqlStatement
    {
        return CommandDefinitionFactory.Create(
            statement,
            param,
            transaction,
            commandTimeout,
            commandType,
            commandFlags,
            cancellationToken
        );
    }
}
