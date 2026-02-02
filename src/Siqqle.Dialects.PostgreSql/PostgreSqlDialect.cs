using Siqqle.Expressions;
using Siqqle.Expressions.Visitors;
using Siqqle.Text;

namespace Siqqle.Dialects.PostgreSql;

/// <summary>
/// Provides support for the <b>PostgreSQL</b> SQL dialect.
/// </summary>
public class PostgreSqlDialect : SqlDialect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostgreSqlDialect"/> class.
    /// </summary>
    public PostgreSqlDialect() { }

    /// <summary>
    /// Formats the specified identifier name for the <b>PostgreSQL</b> dialect.
    /// Returns the identifier name enclosed in double quotes.
    /// </summary>
    /// <param name="identifier">The identifier to format.</param>
    /// <returns>The formatted SQL identifier.</returns>
    public override string FormatIdentifier(string identifier)
    {
        return $"\"{identifier}\"";
    }

    /// <summary>
    /// Formats the specified parameter name for the current SQL dialect.
    /// Returns the parameter name prefixed with a colon.
    /// </summary>
    /// <param name="parameterName">The parameter name to format.</param>
    /// <returns>The formatted parameter name.</returns>
    public override string FormatParameterName(string parameterName)
    {
        return $":{parameterName}";
    }

    /// <summary>
    /// Writes the specified result set limitation for the current SQL dialect.
    /// Writes the <c>LIMIT</c> and <c>OFFSET</c> clauses to the output <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="offset">The row offset at which to start.</param>
    /// <param name="count">The number of rows to fetch.</param>
    public override void WriteLimit(SqlWriter writer, int? offset, int? count)
    {
        if (count.HasValue)
        {
            writer.WriteKeyword(PostgreSqlKeywords.Limit);
            writer.WriteValue(count.Value);
        }

        if (offset.HasValue)
        {
            writer.WriteKeyword(PostgreSqlKeywords.Offset);
            writer.WriteValue(offset.Value);
        }

        writer.WriteDelimiter();
    }

    /// <summary>
    /// Writes the specified call to a stored procedure for the current SQL dialect.
    /// PostgreSQL uses the <c>CALL</c> syntax.
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="visitor">The <see cref="ISqlVisitor"/> to use for visiting the arguments.</param>
    /// <param name="procedureName">The name of the stored procedure to call.</param>
    /// <param name="arguments">The arguments to pass to the stored procedure.</param>
    public override void WriteCall(
        SqlWriter writer,
        ISqlVisitor visitor,
        string procedureName,
        params SqlValue[] arguments
    )
    {
        writer.WriteKeyword(PostgreSqlKeywords.Call);
        writer.WriteIdentifier(procedureName);

        if (arguments != null && arguments.Length > 0)
        {
            writer.WriteRaw("(");
            arguments.Accept(visitor);
            writer.WriteRaw(")");
        }

        writer.WriteDelimiter();
    }

    /// <summary>
    /// Writes a parameter with optional type casting for PostgreSQL.
    /// Supports provider-specific types via SqlParameter&lt;TDbType&gt; for precise PostgreSQL type specification.
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="parameter">The <see cref="SqlParameter"/> to write.</param>
    public override void WriteParameter(SqlWriter writer, SqlParameter parameter)
    {
        writer.WriteParameter(parameter.ParameterName);

        // Check if this is a provider-specific typed parameter
        var parameterType = parameter.GetType();
        if (
            parameterType.IsGenericType
            && parameterType.GetGenericTypeDefinition() == typeof(SqlParameter<>)
        )
        {
            // Get the ProviderDbType property value via reflection
            var providerDbTypeProperty = parameterType.GetProperty("ProviderDbType");
            if (providerDbTypeProperty != null)
            {
                var dbTypeValue = providerDbTypeProperty.GetValue(parameter);
                if (dbTypeValue != null)
                {
                    // Use the enum's string representation as the PostgreSQL type name
                    var typeName = dbTypeValue.ToString().ToLowerInvariant();
                    writer.WriteRaw($"::{typeName}");
                    return;
                }
            }
        }

        // Fall back to standard DbType mapping
        if (parameter.DbType.HasValue)
        {
            switch (parameter.DbType.Value)
            {
                case System.Data.DbType.Guid:
                    writer.WriteRaw("::uuid");
                    break;
                case System.Data.DbType.Xml:
                    writer.WriteRaw("::xml");
                    break;
            }
        }
    }

    /// <summary>
    /// Provides <see cref="SqlKeyword"/> instances for well-known SQL keywords in the PostgreSQL dialect.
    /// </summary>
    public static class PostgreSqlKeywords
    {
        /// <summary>
        /// Represents the PostgreSQL LIMIT keyword.
        /// </summary>
        public static readonly SqlKeyword Limit = "LIMIT";

        /// <summary>
        /// Represents the PostgreSQL OFFSET keyword.
        /// </summary>
        public static readonly SqlKeyword Offset = "OFFSET";

        /// <summary>
        /// Represents the PostgreSQL CALL keyword.
        /// </summary>
        public static readonly SqlKeyword Call = "CALL";
    }
}
