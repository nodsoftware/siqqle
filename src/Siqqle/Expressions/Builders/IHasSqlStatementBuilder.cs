namespace Siqqle.Expressions.Builders;

public interface IHasSqlStatementBuilder<out TBuilder, out TStatement>
    where TBuilder : ISqlStatementBuilder<TStatement>
    where TStatement : SqlStatement
{
    TBuilder Builder { get; }
}
