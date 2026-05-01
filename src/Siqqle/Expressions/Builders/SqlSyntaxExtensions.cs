using System;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

public static class SqlSyntaxExtensions
{
    public static TStatement Go<TStatement>(this ISqlSyntaxEnd<TStatement> syntax)
        where TStatement : SqlStatement
    {
        ArgumentNullException.ThrowIfNull(syntax);

        if (
            syntax is IHasSqlStatementBuilder<ISqlStatementBuilder<TStatement>, TStatement> provider
        )
        {
            return provider.Builder.Build();
        }

        throw new InvalidOperationException(
            "Could not resolve a builder for the specified SQL syntax tree."
        );
    }
}
