using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlSelectSyntax : ISqlSyntaxEnd<SqlSelect>
{
    /// <summary>
    /// Specifies that the SQL SELECT statement should return only distinct (unique) rows.
    /// </summary>
    /// <returns>
    /// The current SQL SELECT builder for method chaining.
    /// </returns>
    ISqlSelectSyntax Distinct();

    /// <summary>
    /// Sets the table from which rows are selected by this SQL SELECT statement.
    /// </summary>
    /// <param name="table">
    /// The table from which rows are selected.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    ISqlSelectFromSyntax From(SqlTable table);

    /// <summary>
    /// Sets the subquery from which rows are selected by this SQL SELECT statement.
    /// </summary>
    /// <param name="query">
    /// The subquery from which rows are selected.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    ISqlSelectFromSyntax From(SqlSubquery query);
}
