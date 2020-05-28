using Siqqle.Syntax;
using System;
using System.Collections.Generic;

namespace Siqqle.Expressions.Builders
{
    public class SqlDeleteBuilder : SqlStatementBuilder<SqlDelete>, ISqlDeleteSyntax
    {
        internal SqlDeleteBuilder()
            : base(new SqlDelete())
        {
        }

        public ISqlDeleteFromSyntax From(SqlTable table)
        {
            Statement.From = new SqlFrom(table);
            return new SqlDeleteFromBuilder(this);
        }
    }
}
