using Siqqle.Expressions;
using Siqqle.Expressions.Visitors;
using Siqqle.Text;

namespace Siqqle.Dialects.MySql;

/// <summary>
/// Provides support for the <b>MySQL</b> SQL dialect.
/// </summary>
public class MySqlDialect : SqlDialect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDialect"/> class.
    /// </summary>
    public MySqlDialect() { }

    /// <summary>
    /// Formats the specified identifier name for the <b>MySQL</b> dialect.
    /// Returns the identifier name enclosed in backtick ('`') characters.
    /// </summary>
    /// <param name="identifier">
    /// The identifier to format.
    /// </param>
    /// <returns>
    /// The formatted SQL identifier.
    /// </returns>
    public override string FormatIdentifier(string identifier)
    {
        return $"`{identifier}`";
    }

    /// <summary>
    /// Formats the specified parameter name for the current SQL dialect.
    /// Returns the parameter name prefixed with an question mark character ('?').
    /// </summary>
    /// <param name="parameterName">
    /// The parameter name to format.
    /// </param>
    /// <returns>
    /// The formatted parameter name.
    /// </returns>
    public override string FormatParameterName(string parameterName)
    {
        return $"?{parameterName}";
    }

    /// <summary>
    /// Writes the specified result set limitation for the current SQL dialect.
    /// Writes the <c>LIMIT <paramref name="offset"/>, <paramref name="count"/></c> clause
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
        writer.WriteKeyword(MySqlKeywords.Limit);
        if (offset != null)
        {
            writer.WriteValue(offset.GetValueOrDefault());
            writer.WriteRaw(",");
        }
        writer.WriteValue(count.GetValueOrDefault(int.MaxValue));
    }

    /// <summary>
    /// Writes a string concatenation expression for MySQL.
    /// MySQL uses the <c>CONCAT(left, right)</c> function for string concatenation.
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="visitor">The <see cref="ISqlVisitor"/> used to visit operands.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    public override void WriteConcatenation(
        SqlWriter writer,
        ISqlVisitor visitor,
        SqlExpression left,
        SqlExpression right
    )
    {
        writer.Write("CONCAT");
        writer.WriteOpenParenthesis();
        left.Accept(visitor);
        writer.WriteComma();
        right.Accept(visitor);
        writer.WriteCloseParenthesis();
    }

    /// <summary>
    /// Returns the MySQL-specific <see cref="SqlDataTypeName"/> for the specified <paramref name="name"/>.
    /// Maps SQL Server-specific date/time types to their MySQL equivalents:
    /// <list type="bullet">
    ///   <item><description><c>DATETIME2</c>, <c>DATETIMEOFFSET</c>, <c>SMALLDATETIME</c> → <c>DATETIME</c></description></item>
    /// </list>
    /// </summary>
    /// <param name="name">The <see cref="SqlDataTypeName"/> to resolve.</param>
    /// <returns>The MySQL-equivalent <see cref="SqlDataTypeName"/>.</returns>
    public override SqlDataTypeName GetDataTypeName(SqlDataTypeName name)
    {
        if (name == SqlDataTypeNames.DateTime2
            || name == SqlDataTypeNames.DateTimeOffset
            || name == SqlDataTypeNames.SmallDateTime)
            return SqlDataTypeNames.DateTime;

        return base.GetDataTypeName(name);
    }

    /// <summary>
    /// Provides <see cref="SqlKeyword"/> instances for well-known SQL keywords in the MySQL dialect.
    /// </summary>
    public static class MySqlKeywords
    {
        /// <summary>
        /// Represents the MySQL LIMIT keyword.
        /// </summary>
        public static readonly SqlKeyword Limit = "LIMIT";
    }
}
