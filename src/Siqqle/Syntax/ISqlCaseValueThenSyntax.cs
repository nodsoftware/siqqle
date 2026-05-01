using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseValueThenSyntax : ISqlCaseValueSyntax, ISqlCaseAsSyntax, ISqlSyntaxEnd
{
    ISqlCaseElseSyntax Else(SqlValue value);
}
