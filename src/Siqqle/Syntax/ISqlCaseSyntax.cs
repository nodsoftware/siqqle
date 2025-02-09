using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseSyntax : ISqlSyntax
{
    ISqlCaseWhenSyntax When(SqlExpression condition);
}
