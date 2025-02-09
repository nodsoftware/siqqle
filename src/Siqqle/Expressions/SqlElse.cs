namespace Siqqle.Expressions;

public class SqlElse(SqlValue value) : SqlClause
{
    public SqlValue Value { get; } = value;

    public override SqlExpressionType ExpressionType => SqlExpressionType.Else;
}
