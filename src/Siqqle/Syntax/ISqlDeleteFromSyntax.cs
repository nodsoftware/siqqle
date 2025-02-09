using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlDeleteFromSyntax : ISqlSyntaxEnd<SqlDelete>
{
    /// <summary>
    /// Sets the predicate used for determining which rows are deleted by this SQL DELETE statement.
    /// </summary>
    /// <param name="predicate">
    /// The predicate used for determining which rows are deleted by this SQL DELETE statement.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    ISqlDeleteWhereSyntax Where(SqlExpression predicate);
}
