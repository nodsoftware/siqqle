using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlSelectJoinBuilder(SqlSelectFromBuilder builder, SqlJoin join)
    : ISqlSelectJoinSyntax
{
    public SqlSelectFromBuilder Builder { get; } = builder;

    public SqlJoin Join { get; } = join;

    public ISqlSelectJoinOnSyntax On(SqlExpression predicate)
    {
        Join.On = new SqlOn(predicate);
        return Builder;
    }
}
