using System;
using System.Linq;
using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders
{
    internal class SqlInsertIntoBuilder : ISqlInsertIntoSyntax, ISqlInsertValuesSyntax, IHasSqlStatementBuilder<SqlInsertBuilder, SqlInsert>
    {
        public SqlInsertIntoBuilder(SqlInsertBuilder builder)
        {
            Builder = builder;
        }

        public SqlInsertBuilder Builder
        {
            get;
        }

        public ISqlInsertValuesSyntax Values(params SqlValue[] values)
        {
            if (values == null || !values.Any()) throw new ArgumentNullException(nameof(values));

            Builder.Statement.Values.Add(values);
            return this;
        }
    }
}