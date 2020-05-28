namespace Siqqle.Expressions.Builders
{
    public interface ISqlStatementBuilder<out TStatement>
        where TStatement : SqlStatement
    {
        TStatement Build();
    }
}