namespace Siqqle.Expressions.Builders;

public interface IHasSqlValueBuilder<out TBuilder, out TValue>
    where TBuilder : ISqlValueBuilder<TValue>
    where TValue : SqlValue
{
    TBuilder Builder { get; }
}
