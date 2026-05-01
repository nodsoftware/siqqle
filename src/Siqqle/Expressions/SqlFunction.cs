using System;
using System.Collections.Generic;

namespace Siqqle.Expressions;

/// <summary>
/// Represents a SQL function call.
/// </summary>
public class SqlFunction : SqlValue
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlFunction"/> class using the specified
    /// function name and arguments.
    /// </summary>
    /// <param name="functionName">
    /// The name of the SQL function to call.
    /// </param>
    /// <param name="alias">
    /// The alias to use for the function.
    /// </param>
    /// <param name="arguments">
    /// The arguments passed to the SQL function.
    /// </param>
    public SqlFunction(
        string functionName,
        IEnumerable<SqlValue> arguments = null,
        string alias = null
    )
    {
        ArgumentNullException.ThrowIfNull(functionName);

        FunctionName = functionName;
        Arguments = arguments;
        Alias = alias;
    }

    /// <summary>
    /// Gets the name of the SQL function.
    /// </summary>
    public string FunctionName { get; private set; }

    /// <summary>
    /// Gets the collection of arguments of the SQL function.
    /// </summary>
    public IEnumerable<SqlValue> Arguments { get; private set; }

    /// <summary>
    /// Gets the alias used for the SQL function.
    /// </summary>
    public string Alias { get; private set; }

    /// <summary>
    /// Gets the expression type of this expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Function;

    /// <summary>
    /// Creates a <see cref="SqlFunction"/> that calls the LOWER function on the specified value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="SqlValue"/> to convert to lowercase.
    /// </param>
    /// <returns>
    /// A <see cref="SqlFunction"/> representing a LOWER() function call.
    /// </returns>
    public static SqlFunction Lower(SqlValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new SqlFunction("LOWER", new[] { value });
    }

    /// <summary>
    /// Creates a <see cref="SqlFunction"/> that calls the UPPER function on the specified value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="SqlValue"/> to convert to uppercase.
    /// </param>
    /// <returns>
    /// A <see cref="SqlFunction"/> representing an UPPER() function call.
    /// </returns>
    public static SqlFunction Upper(SqlValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new SqlFunction("UPPER", new[] { value });
    }
}
