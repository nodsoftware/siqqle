using System.Data;
using System.Threading;
using Dapper;
using Siqqle.Expressions;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class SqlSyntaxEndExtensions
{
    public static CommandDefinition ToCommandDefinition<TStatement>(
        this ISqlSyntaxEnd<TStatement> syntax,
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
            syntax,
            param,
            transaction,
            commandTimeout,
            commandType,
            commandFlags,
            cancellationToken
        );
    }
}
