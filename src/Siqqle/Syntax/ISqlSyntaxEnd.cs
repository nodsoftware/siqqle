using Siqqle.Expressions;

namespace Siqqle.Syntax
{
    public interface ISqlSyntaxEnd
    {
    }

    public interface ISqlSyntaxEnd<TStatement> : ISqlSyntaxEnd
        where TStatement : SqlStatement
    {
    }
}