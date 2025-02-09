using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlCaseAsSyntax : ISqlSyntaxEnd
{
    SqlCase End();
    SqlCase End(string alias);
}
