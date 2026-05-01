namespace Siqqle.Expressions;

/// <summary>
/// Represents a SQL HAVING clause.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SqlHaving"/> class using the
/// specified <paramref name="predicate"/>.
/// </remarks>
/// <param name="predicate">
/// The predicate the SQL HAVING clause uses.
/// </param>
public class SqlHaving(SqlExpression predicate) : SqlWhere(predicate)
{
    /// <summary>
    /// Returns the expression type of this expression.
    /// </summary>
    public override SqlExpressionType ExpressionType => SqlExpressionType.Having;
}
