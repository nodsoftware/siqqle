using Siqqle.Expressions;

namespace Siqqle.Syntax
{
    public interface ISqlUpdateSetSyntax : ISqlUpdateSyntax, ISqlSyntaxEnd<SqlUpdate>
    {
        /// <summary>
        /// Sets the predicate used for determining which rows are updated by this SQL UPDATE statement.
        /// </summary>
        /// <param name="predicate">
        /// The predicate used for determining which rows are updated by this SQL UPDATE statement.
        /// </param>
        /// <returns>
        /// The next grammatical possibilities in the SQL statement.
        /// </returns>
        ISqlUpdateWhereSyntax Where(SqlExpression predicate);
    }
}