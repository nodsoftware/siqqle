using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseThenSyntax : ISqlCaseSyntax, ISqlCaseAsSyntax, ISqlSyntaxEnd
{
    ISqlCaseElseSyntax Else(SqlValue value);
}
