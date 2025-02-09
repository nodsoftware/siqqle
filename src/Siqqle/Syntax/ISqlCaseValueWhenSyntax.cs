using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseValueWhenSyntax
{
    ISqlCaseValueThenSyntax Then(SqlValue value);
}
