using System;
using Siqqle.Expressions;

namespace Siqqle.Syntax;

public interface ISqlDeleteSyntax : ISqlSyntax
{
    /// <summary>
    /// Sets the <paramref name="table"/> from which to delete rows.
    /// </summary>
    /// <param name="table">
    /// The <see cref="SqlTable"/> from which to delete rows.
    /// </param>
    /// <returns>
    /// The next grammatical possibilities in the SQL statement.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="table"/> argument is <see langword="null"/>.
    /// </exception>
    ISqlDeleteFromSyntax From(SqlTable table);
}
