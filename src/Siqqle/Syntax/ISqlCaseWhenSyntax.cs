using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseWhenSyntax
{
    ISqlCaseThenSyntax Then(SqlValue value);
}
