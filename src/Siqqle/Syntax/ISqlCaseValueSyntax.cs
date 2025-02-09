using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseValueSyntax : ISqlSyntax
{
    ISqlCaseValueWhenSyntax When(SqlValue value);
}
