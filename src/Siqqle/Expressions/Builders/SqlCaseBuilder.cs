using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

public class SqlCaseBuilder : SqlValueBuilder<SqlCase>, ISqlCaseSyntax, ISqlCaseValueSyntax
{
    internal SqlCaseBuilder()
        : base(new SqlCase()) { }

    internal SqlCaseBuilder(SqlValue value)
        : base(new SqlCase(value)) { }

    public ISqlCaseWhenSyntax When(SqlExpression condition)
    {
        var when = new SqlWhen(condition);

        Value.AddWhen(when);
        return new SqlCaseWhenBuilder(this, when);
    }

    ISqlCaseValueWhenSyntax ISqlCaseValueSyntax.When(SqlValue value)
    {
        var when = new SqlWhen(value);

        Value.AddWhen(when);
        return new SqlCaseWhenBuilder(this, when);
    }
}
