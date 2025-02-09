using System;
using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlInsertSyntax : ISqlSyntax
{
    /// <summary>
    /// Sets the table into which rows are inserted.
    /// </summary>
    /// <param name="table">
    /// The table into which rows are inserted.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="table"/> argument is <see langword="null"/>.
    /// </exception>
    ISqlInsertIntoSyntax Into(SqlTable table);

    /// <summary>
    /// Sets the table and columns into which rows are inserted.
    /// </summary>
    /// <param name="table">
    /// The table into which rows are inserted.
    /// </param>
    /// <param name="columns">
    /// The columns into which rows are inserted.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="table"/> argument is <see langword="null"/>.
    /// </exception>
    ISqlInsertIntoSyntax Into(SqlTable table, params SqlColumn[] columns);
}
