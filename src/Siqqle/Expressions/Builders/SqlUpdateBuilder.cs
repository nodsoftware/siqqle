using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders;

public class SqlUpdateBuilder : SqlStatementBuilder<SqlUpdate>, ISqlUpdateSyntax
{
    internal SqlUpdateBuilder(SqlTable table)
        : base(new SqlUpdate(table)) { }

    public ISqlUpdateSetSyntax Set(SqlColumn column, SqlValue value)
    {
        return new SqlUpdateSetBuilder(this, column, value);
    }
}
