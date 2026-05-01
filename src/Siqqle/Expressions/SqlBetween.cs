using System;

namespace Siqqle.Expressions;

/// <summary>
/// Represents a BETWEEN expression in SQL.
/// </summary>
public class SqlBetween : SqlExpression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlBetween"/> class using the specified
    /// <paramref name="column"/>, <paramref name="min"/>, and <paramref name="max"/> values.
    /// </summary>
    /// <param name="column">
    /// The column or value expression to test.
    /// </param>
    /// <param name="min">
    /// The minimum value in the range.
    /// </param>
    /// <param name="max">
    /// The maximum value in the range.
    /// </param>
    internal SqlBetween(SqlValue column, SqlValue min, SqlValue max)
    {
        ArgumentNullException.ThrowIfNull(column);
        ArgumentNullException.ThrowIfNull(min);
        ArgumentNullException.ThrowIfNull(max);
        Column = column;
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Returns the expression type of this expression.
    /// </summary>
    public sealed override SqlExpressionType ExpressionType => SqlExpressionType.Between;

    /// <summary>
    /// Gets the column or value expression being tested.
    /// </summary>
    public SqlValue Column { get; private set; }

    /// <summary>
    /// Gets the minimum value in the range.
    /// </summary>
    public SqlValue Min { get; private set; }

    /// <summary>
    /// Gets the maximum value in the range.
    /// </summary>
    public SqlValue Max { get; private set; }
}
