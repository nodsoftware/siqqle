using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders
{
    internal class SqlBuilder
    {
        public ISqlSelectSyntax Select(params SqlColumn[] columns)
        {
            return new SqlSelectBuilder(columns);
        }

        public ISqlSelectSyntax Select(params SqlValue[] values)
        {
            return new SqlSelectBuilder(values);
        }

        /// <summary>
        /// Creates a <see cref="SqlDelete"/> that deletes rows from a table.
        /// </summary>
        /// <returns>
        /// A <see cref="SqlDelete"/> that deletes rows from a table.
        /// </returns>
        public ISqlDeleteSyntax Delete()
        {
            return new SqlDeleteBuilder();
        }

        /// <summary>
        /// Creates a <see cref="SqlInsert"/> that inserts rows into a table.
        /// </summary>
        /// <returns>
        /// A <see cref="SqlInsert"/> that inserts rows into a table.
        /// </returns>
        public static ISqlInsertSyntax Insert()
        {
            return new SqlInsertBuilder();
        }
    }
}
