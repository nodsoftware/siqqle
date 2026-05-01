using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

internal class SqlCaseWhenBuilder(SqlCaseBuilder builder, SqlWhen when)
    : ISqlCaseWhenSyntax,
        ISqlCaseValueWhenSyntax,
        ISqlCaseThenSyntax,
        ISqlCaseValueThenSyntax,
        ISqlCaseAsSyntax,
        ISqlCaseElseSyntax,
        IHasSqlValueBuilder<SqlCaseBuilder, SqlCase>
{
    public SqlCaseBuilder Builder { get; } = builder;

    public SqlWhen When { get; } = when;

    public ISqlCaseThenSyntax Then(SqlValue value)
    {
        When.Value = value;
        return this;
    }

    ISqlCaseValueThenSyntax ISqlCaseValueWhenSyntax.Then(SqlValue value)
    {
        When.Value = value;
        return this;
    }

    ISqlCaseWhenSyntax ISqlCaseSyntax.When(SqlExpression condition)
    {
        var when = new SqlWhen(condition);

        Builder.Value.AddWhen(when);
        return new SqlCaseWhenBuilder(Builder, when);
    }

    ISqlCaseValueWhenSyntax ISqlCaseValueSyntax.When(SqlValue value)
    {
        var when = new SqlWhen(value);

        Builder.Value.AddWhen(when);
        return new SqlCaseWhenBuilder(Builder, when);
    }

    public ISqlCaseElseSyntax Else(SqlValue value)
    {
        Builder.Value.Else = new SqlElse(value);
        return this;
    }

    public SqlCase End()
    {
        return Builder.Value;
    }

    public SqlCase End(string alias)
    {
        Builder.Value.Alias = alias;
        return Builder.Value;
    }

    SqlCase ISqlCaseAsSyntax.End()
    {
        return Builder.Value;
    }

    SqlCase ISqlCaseAsSyntax.End(string alias)
    {
        Builder.Value.Alias = alias;
        return Builder.Value;
    }
}
