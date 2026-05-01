using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSelectOrderBySyntax : ISqlSyntaxEnd<SqlSelect>
{
    ISqlSelectThenBySyntax OrderBy(
        SqlColumn column,
        SqlSortOrder sortOrder = SqlSortOrder.Ascending
    );

    ISqlSelectThenBySyntax OrderBy(SqlSort sorting);
}
