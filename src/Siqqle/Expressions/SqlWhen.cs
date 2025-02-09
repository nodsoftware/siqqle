using System;

namespace Siqqle.Expressions;

public class SqlWhen : SqlClause
{
    public SqlWhen(SqlExpression condition)
    {
        Condition = condition ?? throw new ArgumentNullException(nameof(condition));
    }

    public SqlWhen(SqlValue value)
    {
        Condition = value ?? throw new ArgumentNullException(nameof(value));
    }

    public SqlExpression Condition { get; }

    public SqlValue Value { get; internal set; }

    public override SqlExpressionType ExpressionType => SqlExpressionType.When;
}
