namespace Siqqle.Expressions.Builders;

public abstract class SqlStatementBuilder<TStatement>(TStatement statement)
    : ISqlStatementBuilder<TStatement>,
        IHasSqlStatementBuilder<ISqlStatementBuilder<TStatement>, TStatement>
    where TStatement : SqlStatement
{
    public ISqlStatementBuilder<TStatement> Builder => this;

    protected internal TStatement Statement { get; } = statement;

    public virtual TStatement Build()
    {
        return Statement;
    }
}
