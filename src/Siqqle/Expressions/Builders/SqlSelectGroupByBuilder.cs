using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlSelectGroupByBuilder
    : ISqlSelectGroupBySyntax,
        ISqlSelectHavingSyntax,
        ISqlSelectThenBySyntax,
        ISqlSelectLimitClause,
        IHasSqlStatementBuilder<SqlSelectBuilder, SqlSelect>
{
    public SqlSelectGroupByBuilder(SqlSelectBuilder builder, SqlColumn column)
    {
        Builder = builder;
        Builder.Statement.GroupBy = new SqlGroupBy(column);
    }

    public SqlSelectBuilder Builder { get; }

    public ISqlSelectHavingSyntax Having(SqlExpression predicate)
    {
        Builder.Statement.GroupBy.Having = new SqlHaving(predicate);
        return this;
    }

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
        Builder.Statement.OrderBy.Add(sorting);
        return this;
    }
}
