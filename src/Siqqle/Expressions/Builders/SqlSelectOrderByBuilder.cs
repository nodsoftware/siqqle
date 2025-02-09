using System;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlSelectOrderByBuilder
    : ISqlSelectThenBySyntax,
        ISqlSelectLimitClause,
        IHasSqlStatementBuilder<SqlSelectBuilder, SqlSelect>
{
    public SqlSelectOrderByBuilder(
        SqlSelectBuilder builder,
        SqlColumn column,
        SqlSortOrder sortOrder
    )
    {
        Builder = builder;
        OrderBy(column, sortOrder);
    }

    public SqlSelectOrderByBuilder(SqlSelectBuilder builder, SqlSort sorting)
    {
        Builder = builder;
        OrderBy(sorting);
    }

    public SqlSelectBuilder Builder { get; }

    public ISqlSelectLimitClause Limit(int offset, int count)
    {
        Builder.Statement.Limit = new SqlLimit(offset, count);
        return this;
    }

    public ISqlSelectLimitClause Limit(int count)
    {
        Builder.Statement.Limit = new SqlLimit(null, count);
        return this;
    }

    public ISqlSelectThenBySyntax OrderBy(
        SqlColumn column,
        SqlSortOrder sortOrder = SqlSortOrder.Ascending
    )
    {
        return OrderBy(new SqlSort(column, sortOrder));
    }

    public ISqlSelectThenBySyntax OrderBy(SqlSort sorting)
    {
        ArgumentNullException.ThrowIfNull(sorting);

        Builder.Statement.OrderBy.Add(sorting);
        return this;
    }
}
