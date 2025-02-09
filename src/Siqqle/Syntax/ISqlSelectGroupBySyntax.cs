using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSelectGroupBySyntax : ISqlSelectOrderBySyntax
{
    ISqlSelectHavingSyntax Having(SqlExpression predicate);
}
