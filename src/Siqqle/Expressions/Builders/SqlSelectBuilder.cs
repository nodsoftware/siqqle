using System;
using System.Collections.Generic;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

public class SqlSelectBuilder : SqlStatementBuilder<SqlSelect>, ISqlSelectSyntax
{
    internal SqlSelectBuilder(IEnumerable<SqlValue> columns)
        : base(new SqlSelect(columns)) { }

    public override SqlSelect Build()
    {
        return Statement;
    }

    public ISqlSelectSyntax Distinct()
    {
        Statement.Distinct = true;
        return this;
    }

    public ISqlSelectFromSyntax From(SqlTable table)
    {
        Statement.From = new SqlFrom(table);
        return new SqlSelectFromBuilder(this);
    }

    public ISqlSelectFromSyntax From(SqlSubquery query)
    {
        ArgumentNullException.ThrowIfNull(query);
        if (query.Alias == null)
            throw new ArgumentException(
                "An alias is required for subqueries in a FROM clause.",
                nameof(query)
            );

        Statement.From = new SqlFrom(query);
        return new SqlSelectFromBuilder(this);
    }
}
