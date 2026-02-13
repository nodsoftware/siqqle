using System;
using Siqqle.Expressions.Visitors;

namespace Siqqle.Expressions;

/// <summary>
/// Represents an EXISTS expression in SQL.
/// </summary>
public class SqlExists : SqlExpression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlExists"/> class using the
    /// specified <paramref name="subquery"/> and <paramref name="isNegated"/> flag.
    /// </summary>
    /// <param name="subquery">
    /// The subquery to test for existence.
    /// </param>
    /// <param name="isNegated">
    /// A value indicating whether this is a NOT EXISTS expression.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="subquery"/> argument is <see langword="null"/>.
    /// </exception>
    public SqlExists(SqlSubquery subquery, bool isNegated = false)
    {
        ArgumentNullException.ThrowIfNull(subquery);

        Subquery = subquery;
        IsNegated = isNegated;
    }

    /// <summary>
    /// Gets the subquery to test for existence.
    /// </summary>
    public SqlSubquery Subquery { get; }

    /// <summary>
    /// Gets a value indicating whether this is a NOT EXISTS expression.
    /// </summary>
    public bool IsNegated { get; }

    /// <inheritdoc/>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Exists;

    /// <inheritdoc/>
    public override void Accept(ISqlVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}
