using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders
{
    public class SqlInsertBuilder : SqlStatementBuilder<SqlInsert>, ISqlInsertSyntax
    {
        internal SqlInsertBuilder()
            : base(new SqlInsert())
        {
        }

        public ISqlInsertIntoSyntax Into(SqlTable table)
        {
            Statement.Into = new SqlInto(table);
            return new SqlInsertIntoBuilder(this);
        }

        public ISqlInsertIntoSyntax Into(SqlTable table, params SqlColumn[] columns)
        {
            Statement.Into = new SqlInto(table);
            Statement.Columns = columns;
            return new SqlInsertIntoBuilder(this);
        }
    }
}
