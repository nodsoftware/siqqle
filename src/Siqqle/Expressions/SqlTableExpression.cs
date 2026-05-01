namespace Siqqle.Expressions;

/// <summary>
/// Represents a SQL table expression.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SqlTableExpression"/> class using
/// the specified <paramref name="alias"/>.
/// </remarks>
/// <param name="alias">
/// The alias to use for the table expression.
/// </param>
public abstract class SqlTableExpression(string alias) : SqlExpression
{
    /// <summary>
    /// Gets the alias used for the table.
    /// </summary>
    public string Alias { get; private set; } = alias;

    /// <summary>
    /// Gets the expression type of this expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Table;
}
