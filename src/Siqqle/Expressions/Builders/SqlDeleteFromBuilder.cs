using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlDeleteFromBuilder(SqlDeleteBuilder builder)
    : ISqlDeleteFromSyntax,
        ISqlDeleteWhereSyntax,
        IHasSqlStatementBuilder<SqlDeleteBuilder, SqlDelete>
{
    public SqlDeleteBuilder Builder { get; } = builder;

    public ISqlDeleteWhereSyntax Where(SqlExpression predicate)
    {
        Builder.Statement.Where = new SqlWhere(predicate);
        return this;
    }
}
