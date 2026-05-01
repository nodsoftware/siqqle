namespace Siqqle.Expressions.Builders;

public interface ISqlValueBuilder<out TValue>
    where TValue : SqlValue
{
    TValue Build();
}
