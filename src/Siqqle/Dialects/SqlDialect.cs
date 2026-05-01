using System;
using Siqqle.Expressions;
using Siqqle.Expressions.Visitors;
using Siqqle.Text;

namespace Siqqle.Dialects;

/// <summary>
/// Provides the base class for classes providing support for specific SQL dialects.
/// </summary>
public class SqlDialect
{
    private static SqlDialect _current;

    static SqlDialect()
    {
        Default = new SqlDialect();
        Current = Default;
    }

    /// <summary>
    /// Gets a reference to the default SQL dialect.
    /// </summary>
    public static SqlDialect Default { get; }

    /// <summary>
    /// Gets the currently used <see cref="SqlDialect"/>.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="Default"/>. Can be changed process-wide by calling <see cref="UseDialect"/>.
    /// In concurrent applications, prefer passing the dialect explicitly to
    /// <c>ToSql(SqlDialect)</c> overloads or using the <c>Siqqle.Extensions.DependencyInjection</c> package.
    /// </remarks>
    public static SqlDialect Current
    {
        get { return _current; }
        private set { _current = value ?? Default; }
    }

    /// <summary>
    /// Sets the process-wide default <see cref="SqlDialect"/>.
    /// </summary>
    /// <param name="dialect">
    /// The <see cref="SqlDialect"/> to use as the process-wide default.
    /// Pass <see langword="null"/> to restore <see cref="Default"/>.
    /// </param>
    /// <remarks>
    /// This method is not thread-safe and should only be called once at application startup before
    /// any queries are executed. In concurrent applications, prefer passing the dialect explicitly to
    /// <c>ToSql(SqlDialect)</c> overloads or using the <c>Siqqle.Extensions.DependencyInjection</c> package.
    /// </remarks>
    [Obsolete(
        "Setting a global process-level dialect is not thread-safe in concurrent applications. Pass the dialect explicitly to ToSql() overloads or use the Siqqle.Extensions.DependencyInjection package instead."
    )]
    public static void UseDialect(SqlDialect dialect)
    {
        Current = dialect;
    }

    /// <summary>
    /// Formats the specified identifier name for the current SQL dialect. The default implementation
    /// returns the identifier name enclosed in square brackets.
    /// </summary>
    /// <param name="identifier">
    /// The identifier to format.
    /// </param>
    /// <returns>
    /// The formatted SQL identifier.
    /// </returns>
    public virtual string FormatIdentifier(string identifier)
    {
        return $"[{identifier}]";
    }

    /// <summary>
    /// Formats the specified parameter name for the current SQL dialect. The default implementation
    /// returns the parameter name prefixed with an 'at' character ('@').
    /// </summary>
    /// <param name="parameterName">
    /// The parameter name to format.
    /// </param>
    /// <returns>
    /// The formatted parameter name.
    /// </returns>
    public virtual string FormatParameterName(string parameterName)
    {
        return $"@{parameterName}";
    }

    /// <summary>
    /// Formats the specified string for the current SQL dialect. The default implementation
    /// returns the table name enclosed in single quotes.
    /// </summary>
    /// <param name="value">
    /// The SQL string to format.
    /// </param>
    /// <returns>
    /// The formatted SQL string.
    /// </returns>
    public virtual string FormatString(string value)
    {
        return $"'{value.Replace("'", "''")}'";
    }

    /// <summary>
    /// Writes a call to the specified stored procedure with the specified arguments for the current SQL dialect. The default
    /// implementation uses the <c>CALL <paramref name="procedureName"/> (<paramref name="arguments"/>)</c> syntax.
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
    /// <remarks>
    /// The <paramref name="visitor"/> must be used to visit the arguments to ensure that they are written correctly.
    /// Use the <see cref="SqlVisitorExtensions.Accept(System.Collections.Generic.IEnumerable{SqlValue}, ISqlVisitor)"/> method on the arguments to visit them. For example:
    /// <code>
    /// arguments.Accept(visitor);
    /// </code>
    /// </remarks>
    public virtual void WriteCall(
        SqlWriter writer,
        ISqlVisitor visitor,
        string procedureName,
        params SqlValue[] arguments
    )
    {
        writer.WriteKeyword(SqlKeywords.Call);
        writer.WriteIdentifier(procedureName);
        writer.WriteOpenParenthesis();

        arguments.Accept(visitor);

        writer.WriteCloseParenthesis();
    }

    /// <summary>
    /// Writes the specified result set limitation for the current SQL dialect. The default
    /// implementation uses the <c>OFFSET <paramref name="offset"/> ROWS FETCH FIRST <paramref name="count"/> ROWS ONLY</c>
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
    public virtual void WriteLimit(SqlWriter writer, int? offset, int? count)
    {
        if (offset != null)
        {
            writer.WriteKeyword(SqlKeywords.Offset);
            writer.WriteValue(offset.Value);
            writer.WriteKeyword(SqlKeywords.Rows);
        }

        if (count != null)
        {
            writer.WriteKeyword(SqlKeywords.Fetch);
            writer.WriteKeyword(SqlKeywords.First);
            writer.WriteValue(count.Value);
            writer.WriteKeyword(SqlKeywords.Rows);
            writer.WriteKeyword(SqlKeywords.Only);
        }
    }

    /// <summary>
    /// Writes the specified arithmetic operator to the output stream. The default implementation
    /// writes the standard SQL arithmetic symbols (<c>+</c>, <c>-</c>, <c>*</c>, <c>/</c>, <c>%</c>).
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="operator">The <see cref="SqlBinaryOperator"/> to write.</param>
    /// <remarks>
    /// Override this method in a dialect subclass to customize arithmetic operator rendering.
    /// A common case is string concatenation: SQL Server uses <c>+</c>, PostgreSQL uses <c>||</c>,
    /// and MySQL requires <c>CONCAT()</c>.
    /// </remarks>
    /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
    /// Thrown when <paramref name="operator"/> is not a recognized arithmetic operator.
    /// </exception>
    public virtual void WriteArithmeticOperator(SqlWriter writer, SqlBinaryOperator @operator)
    {
        switch (@operator)
        {
            case SqlBinaryOperator.Add:
                writer.Write("+");
                break;
            case SqlBinaryOperator.Subtract:
                writer.Write("-");
                break;
            case SqlBinaryOperator.Multiply:
                writer.Write("*");
                break;
            case SqlBinaryOperator.Divide:
                writer.Write("/");
                break;
            case SqlBinaryOperator.Modulo:
                writer.Write("%");
                break;
            default:
                throw new System.ComponentModel.InvalidEnumArgumentException(
                    nameof(@operator),
                    (int)@operator,
                    typeof(SqlBinaryOperator)
                );
        }
    }

    /// <summary>
    /// Writes a string concatenation expression for the current SQL dialect.
    /// The default implementation emits <c>left || right</c> (standard SQL / PostgreSQL).
    /// </summary>
    /// <param name="writer">The <see cref="SqlWriter"/> to write to.</param>
    /// <param name="visitor">The <see cref="ISqlVisitor"/> used to visit operands.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <remarks>
    /// Override in dialect subclasses to produce dialect-specific concatenation:
    /// SQL Server emits <c>left + right</c>; MySQL emits <c>CONCAT(left, right)</c>.
    /// </remarks>
    public virtual void WriteConcatenation(
        SqlWriter writer,
        ISqlVisitor visitor,
        SqlExpression left,
        SqlExpression right
    )
    {
        left.Accept(visitor);
        writer.Write("|");
        writer.WriteRaw("|");
        right.Accept(visitor);
    }

    /// <summary>
    /// Writes a parameter with optional type casting for the current SQL dialect. The default
    /// implementation writes the parameter name without any additional casting.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="SqlWriter"/> to write to.
    /// </param>
    /// <param name="parameter">
    /// The <see cref="SqlParameter"/> to write.
    /// </param>
    public virtual void WriteParameter(SqlWriter writer, SqlParameter parameter)
    {
        writer.WriteParameter(parameter.ParameterName);
    }
}
