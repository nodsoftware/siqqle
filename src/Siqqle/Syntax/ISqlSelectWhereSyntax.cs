using Siqqle.Expressions;

namespace Siqqle.Syntax
{
    public interface ISqlSelectWhereSyntax : ISqlSelectOrderBySyntax
    {
        ISqlSelectGroupBySyntax GroupBy(SqlColumn column);
    }
}