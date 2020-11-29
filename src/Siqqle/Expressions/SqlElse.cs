namespace Siqqle.Expressions
{
    public class SqlElse : SqlClause
    {
        public SqlElse(SqlValue value)
        {
            Value = value;
        }

        public SqlValue Value
        {
            get;
        }

        public override SqlExpressionType ExpressionType
            => SqlExpressionType.Else;
    }
}