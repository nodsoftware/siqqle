namespace Siqqle.Syntax
{
    public interface ISqlSelectThenBySyntax : ISqlSelectOrderBySyntax
    {
        ISqlSelectLimitClause Limit(int offset, int count);

        /// <summary>
        /// Adds a SQL LIMIT clause to this SQL SELECT clause.
        /// </summary>
        /// <param name="count">
        /// The number of rows to fetch.
        /// </param>
        /// <returns>
        /// The next grammatical possibilities in the SQL statement.
        /// </returns>
        ISqlSelectLimitClause Limit(int count);
    }
}