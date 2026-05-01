using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSelectFromSyntax : ISqlSelectOrderBySyntax
{
    ISqlSelectWhereSyntax Where(SqlExpression predicate);

    ISqlSelectJoinSyntax Join(SqlTable table);
    ISqlSelectJoinSyntax InnerJoin(SqlTable table);
    ISqlSelectJoinSyntax LeftJoin(SqlTable table);
    ISqlSelectJoinSyntax RightJoin(SqlTable table);

    ISqlSelectGroupBySyntax GroupBy(SqlColumn column);
}
