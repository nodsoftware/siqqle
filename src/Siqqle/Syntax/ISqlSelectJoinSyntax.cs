using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSelectJoinSyntax : ISqlSyntax
{
    ISqlSelectJoinOnSyntax On(SqlExpression predicate);
}
