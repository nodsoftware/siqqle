using System;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlSelectFromBuilder(SqlSelectBuilder builder)
    : ISqlSelectFromSyntax,
        ISqlSelectJoinOnSyntax,
        ISqlSelectWhereSyntax,
        IHasSqlStatementBuilder<SqlSelectBuilder, SqlSelect>
{
    public SqlSelectBuilder Builder { get; } = builder;

    public ISqlSelectGroupBySyntax GroupBy(SqlColumn column)
    {
        return new SqlSelectGroupByBuilder(Builder, column);
    }

    public ISqlSelectJoinSyntax InnerJoin(SqlTable table)
    {
        return Join(SqlJoinType.Inner, table);
    }

    public ISqlSelectJoinSyntax Join(SqlTable table)
    {
        return Join(SqlJoinType.Default, table);
    }

    public ISqlSelectJoinSyntax LeftJoin(SqlTable table)
    {
        return Join(SqlJoinType.Left, table);
    }

    public ISqlSelectJoinSyntax RightJoin(SqlTable table)
    {
        return Join(SqlJoinType.Right, table);
    }

    private SqlSelectJoinBuilder Join(SqlJoinType type, SqlTable table)
    {
        ArgumentNullException.ThrowIfNull(table);
        var join = new SqlJoin(type, table);
        Builder.Statement.From.AddJoin(join);
        return new SqlSelectJoinBuilder(this, join);
    }

    public ISqlSelectThenBySyntax OrderBy(
        SqlColumn column,
        SqlSortOrder sortOrder = SqlSortOrder.Ascending
    )
    {
        return new SqlSelectOrderByBuilder(Builder, column, sortOrder);
    }

    public ISqlSelectThenBySyntax OrderBy(SqlSort sorting)
    {
        return new SqlSelectOrderByBuilder(Builder, sorting);
    }

    public ISqlSelectWhereSyntax Where(SqlExpression predicate)
    {
        Builder.Statement.Where = new SqlWhere(predicate);
        return this;
    }
}
