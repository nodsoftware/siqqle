using System;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlInsertIntoBuilder(SqlInsertBuilder builder)
    : ISqlInsertIntoSyntax,
        ISqlInsertValuesSyntax,
        IHasSqlStatementBuilder<SqlInsertBuilder, SqlInsert>
{
    public SqlInsertBuilder Builder { get; } = builder;

    public ISqlInsertValuesSyntax Values(params SqlValue[] values)
    {
        if (values == null || values.Length == 0)
            throw new ArgumentNullException(nameof(values));

        Builder.Statement.Values.Add(values);
        return this;
    }
}
