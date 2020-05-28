using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders
{
    internal class SqlUpdateSetBuilder : ISqlUpdateSetSyntax, ISqlUpdateWhereSyntax, IHasSqlStatementBuilder<SqlUpdateBuilder, SqlUpdate>
    {
        public SqlUpdateSetBuilder(SqlUpdateBuilder builder, SqlColumn column, SqlValue value)
        {
            Builder = builder;
            Set(column, value);
        }

        public SqlUpdateBuilder Builder
        {
            get;
        }

        public ISqlUpdateSetSyntax Set(SqlColumn column, SqlValue value)
        {
            Builder.Statement.Set.Add(new SqlAssign(column, value));
            return this;
        }

        public ISqlUpdateWhereSyntax Where(SqlExpression predicate)
        {
            Builder.Statement.Where = new SqlWhere(predicate);
            return this;
        }
    }
}