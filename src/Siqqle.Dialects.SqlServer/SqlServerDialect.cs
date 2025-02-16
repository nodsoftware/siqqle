using Siqqle.Expressions;
using Siqqle.Expressions.Visitors;
using Siqqle.Text;

namespace Siqqle.Dialects.SqlServer;

/// <summary>
/// Provides support for the <b>SQL Server (T-SQL)</b> SQL dialect.
/// </summary>
public class SqlServerDialect : SqlDialect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlServerDialect"/> class.
    /// </summary>
    public SqlServerDialect() { }

    /// <summary>
    /// Writes the specified result set limitation for the current SQL dialect.
    /// Writes the <c>OFFSET <paramref name="offset"/> ROWS FETCH FIRST <paramref name="count"/> ROWS ONLY</c> clause
    /// to the output <paramref name="writer"/>.
    /// syntax.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="SqlWriter"/> to write to.
    /// </param>
    /// <param name="offset">
    /// The row offset at which to start.
    /// </param>
    /// <param name="count">
    /// The number of rows to fetch.
    /// </param>
    public override void WriteLimit(SqlWriter writer, int? offset, int? count)
    {
        // SQL Server requires an offset...
        if (offset == null && count != null)
            offset = 0;

        base.WriteLimit(writer, offset, count);
    }

    /// <summary>
    /// Writes the specified call to a stored procedure for the current SQL dialect.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="SqlWriter"/> to write to.
    /// </param>
    /// <param name="visitor">
    /// The <see cref="ISqlVisitor"/> to use for visiting the arguments.
    /// </param>
    /// <param name="procedureName">
    /// The name of the stored procedure to call.
    /// </param>
    /// <param name="arguments">
    /// The arguments to pass to the stored procedure.
    /// </param>
    public override void WriteCall(
        SqlWriter writer,
        ISqlVisitor visitor,
        string procedureName,
        params SqlValue[] arguments
    )
    {
        writer.WriteKeyword(SqlServerKeywords.Execute);
        writer.WriteIdentifier(procedureName);

        arguments?.Accept(visitor);

        writer.WriteDelimiter();
    }

    /// <summary>
    /// Provides <see cref="SqlKeyword"/> instances for well-known SQL keywords in the MySQL dialect.
    /// </summary>
    public static class SqlServerKeywords
    {
        /// <summary>
        /// Represents the SQL Server EXEC keyword.
        /// </summary>
        public static readonly SqlKeyword Execute = "EXEC";
    }
}
