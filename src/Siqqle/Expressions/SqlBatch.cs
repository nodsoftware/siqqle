using System;
using System.Collections.Generic;
using System.Linq;

namespace Siqqle.Expressions;

/// <summary>
/// Represents a batch of statements.
/// </summary>
public class SqlBatch : SqlStatement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlBatch"/> class using the specified
    /// SQL statements.
    /// </summary>
    /// <param name="statements">
    /// A collection of <see cref="SqlStatement"/> instances to concatenate.
    /// </param>
    public SqlBatch(params SqlStatement[] statements)
        : this((IEnumerable<SqlStatement>)statements) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlBatch"/> class using the specified
    /// SQL SELECT statements.
    /// </summary>
    /// <param name="statements">
    /// A collection of <see cref="SqlStatement"/> instances to batch.
    /// </param>
    public SqlBatch(IEnumerable<SqlStatement> statements)
    {
        ArgumentNullException.ThrowIfNull(statements);
        if (!statements.Any())
            throw new ArgumentException(
                "At least one statement should be provided.",
                nameof(statements)
            );

        Statements = statements;
    }

    /// <summary>
    /// Gets the collection of <see cref="SqlStatement"/> statements concatenated by this <see cref="SqlBatch"/>.
    /// </summary>
    public IEnumerable<SqlStatement> Statements { get; }

    /// <summary>
    /// Returns the expression type of this expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Batch;
}
