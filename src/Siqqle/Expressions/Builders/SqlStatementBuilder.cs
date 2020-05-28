namespace Siqqle.Expressions.Builders
{

    public abstract class SqlStatementBuilder<TStatement> : ISqlStatementBuilder<TStatement>, IHasSqlStatementBuilder<ISqlStatementBuilder<TStatement>, TStatement>
        where TStatement : SqlStatement 
    {
        protected SqlStatementBuilder(TStatement statement)
        {
            Statement = statement;
        }

        public ISqlStatementBuilder<TStatement> Builder
            => this;

        protected internal TStatement Statement
        {
            get;
        }

        public virtual TStatement Build()
        {
            return Statement;
        }
    }
}