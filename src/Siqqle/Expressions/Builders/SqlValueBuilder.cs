namespace Siqqle.Expressions.Builders
{
    public class SqlValueBuilder<TValue> : ISqlValueBuilder<TValue>, IHasSqlValueBuilder<ISqlValueBuilder<TValue>, TValue>
        where TValue: SqlValue
    {
        protected SqlValueBuilder(TValue value)
        {
            Value = value;
        }

        public ISqlValueBuilder<TValue> Builder
            => this;

        protected internal TValue Value
        {
            get;
        }

        public virtual TValue Build()
        {
            return Value;
        }
    }
}