using Siqqle.Syntax;
using System;

namespace Siqqle.Expressions.Builders
{
    internal class SqlDeleteFromBuilder : ISqlDeleteFromSyntax, ISqlDeleteWhereSyntax, IHasSqlStatementBuilder<SqlDeleteBuilder, SqlDelete>
    {
        public SqlDeleteFromBuilder(SqlDeleteBuilder builder)
        {
            Builder = builder;
        }

        public SqlDeleteBuilder Builder
        {
            get;
        }

        public ISqlDeleteWhereSyntax Where(SqlExpression predicate)
        {
            Builder.Statement.Where = new SqlWhere(predicate);
            return this;
        }
    }
}