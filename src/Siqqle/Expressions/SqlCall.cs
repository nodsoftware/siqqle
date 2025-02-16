using System;
using System.Collections.Generic;

namespace Siqqle.Expressions;

/// <summary>
/// Represents a SQL call statement.
/// </summary>
public class SqlCall : SqlStatement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlCall"/> class.
    /// </summary>
    /// <param name="procedureName">The name of the stored procedure to call.</param>
    /// <param name="arguments">The arguments to pass to the stored procedure.</param>
    internal SqlCall(string procedureName, IEnumerable<SqlValue> arguments)
    {
        ProcedureName = procedureName ?? throw new ArgumentNullException(nameof(procedureName));
        Arguments = arguments;
    }

    /// <summary>
    /// Gets the name of the stored procedure to call.
    /// </summary>
    public string ProcedureName { get; }

    /// <summary>
    /// Gets the arguments to pass to the stored procedure.
    /// </summary>
    public IEnumerable<SqlValue> Arguments { get; }

    /// <summary>
    /// Gets the type of the SQL expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Call;
}
