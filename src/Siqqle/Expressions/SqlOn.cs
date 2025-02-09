namespace Siqqle.Expressions;

/// <summary>
/// Represents a SQL ON clause.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SqlOn"/> class using the
/// specified <paramref name="predicate"/>.
/// </remarks>
/// <param name="predicate">
/// The predicate the SQL ON clause uses.
/// </param>
public class SqlOn(SqlExpression predicate) : SqlWhere(predicate)
{
    /// <summary>
    /// Returns the expression type of this expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.On;
}
