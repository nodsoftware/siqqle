using System;
using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlInsertValuesSyntax : ISqlSyntaxEnd<SqlInsert>
{
    /// <summary>
    /// Sets the values inserted into the table.
    /// </summary>
    /// <param name="values">
    /// The values to insert into the table.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="values"/> argument is <see langword="null"/> or
    /// an empty array.
    /// </exception>
    ISqlInsertValuesSyntax Values(params SqlValue[] values);
}
