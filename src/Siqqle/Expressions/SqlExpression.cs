using System;
using System.Linq;
using Siqqle.Expressions.Builders;
using Siqqle.Expressions.Visitors;
using Siqqle.Syntax;

namespace Siqqle.Expressions;

/// <summary>
/// Provides the base class for SQL expressions and provides <see langword="static"/> factory methods
/// for creating specific SQL expressions.
/// </summary>
public abstract class SqlExpression : ISqlVisitable
{
    public static ISqlCaseSyntax Case()
    {
        return new SqlCaseBuilder();
    }

    public static ISqlCaseValueSyntax Case(SqlValue value)
    {
        return new SqlCaseBuilder(value);
    }

    public static ISqlCaseValueSyntax Case(SqlColumn value)
    {
        return new SqlCaseBuilder(value);
    }

    /// <summary>
    /// Creates a <see cref="SqlCast"/> that represents a conversion of the specified <paramref name="value"/>
    /// to the specified data type.
    /// </summary>
    /// <param name="value">
    /// The <see cref="SqlValue"/> to convert.
    /// </param>
    /// <param name="dataType">
    /// The <see cref="SqlDataType"/> to convert to.
    /// </param>
    /// <returns>
    /// A <see cref="SqlCast"/> that represents a conversion of the specified <paramref name="value"/>
    /// to the specified data type.
    /// </returns>
    public static SqlCast Cast(SqlValue value, SqlDataType dataType)
    {
        return new SqlCast(value, dataType);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a logical <b>AND</b> operation.
    /// </summary>
    /// <param name="left">
    /// A <see cref="SqlExpression"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="right">
    /// A <see cref="SqlExpression"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents a logical <b>AND</b> operation between
    /// the specified <paramref name="left"/> and <paramref name="right"/> operands.
    /// </returns>
    public static SqlBinaryExpression And(SqlExpression left, SqlExpression right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.And, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a logical <b>OR</b> operation.
    /// </summary>
    /// <param name="left">
    /// A <see cref="SqlExpression"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="right">
    /// A <see cref="SqlExpression"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents a logical <b>OR</b> operation between
    /// the specified <paramref name="left"/> and <paramref name="right"/> operands.
    /// </returns>
    public static SqlBinaryExpression Or(SqlExpression left, SqlExpression right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Or, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlUnaryExpression"/> that represents a <c>null</c> value comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlUnaryExpression"/> that represents a null value comparison on
    /// the value of the specified <paramref name="column"/>.
    /// </returns>
    public static SqlUnaryExpression IsNull(SqlColumn column)
    {
        return new SqlUnaryExpression(column, SqlUnaryOperator.IsNull);
    }

    /// <summary>
    /// Creates a <see cref="SqlUnaryExpression"/> that represents a <c>null</c> value comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlUnaryExpression"/> that represents a <c>not-null</c> value comparison on
    /// the value of the specified <paramref name="column"/>.
    /// </returns>
    public static SqlUnaryExpression IsNotNull(SqlColumn column)
    {
        return new SqlUnaryExpression(column, SqlUnaryOperator.IsNotNull);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an equality comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an equality comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression Equal(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(column, SqlBinaryOperator.Equal, value ?? SqlConstant.Null);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an equality comparison
    /// between two <see cref="SqlFunction"/> expressions.
    /// </summary>
    /// <param name="left">
    /// A <see cref="SqlFunction"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="right">
    /// A <see cref="SqlFunction"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an equality comparison between
    /// the specified <paramref name="left"/> and <paramref name="right"/> functions.
    /// </returns>
    public static SqlBinaryExpression Equal(SqlFunction left, SqlFunction right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Equal, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an inequality comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an inequality comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression NotEqual(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.NotEqual,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "greater than" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "greater than" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression GreaterThan(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.GreaterThan,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "greater than or equal to" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "greater than or equal to" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression GreaterThanOrEqual(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.GreaterThanOrEqual,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "less than" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "less than" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression LessThan(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.LessThan,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "less than or equal to" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "less than or equal to" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression LessThanOrEqual(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.LessThanOrEqual,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "like" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "like" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression Like(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(column, SqlBinaryOperator.Like, value ?? SqlConstant.Null);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a "not like" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="value">
    /// A <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents a "not like" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="value"/> argument, it
    /// will be automatically converted to <see cref="SqlConstant.Null"/>.
    /// </remarks>
    public static SqlBinaryExpression NotLike(SqlColumn column, SqlValue value)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.NotLike,
            value ?? SqlConstant.Null
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "in" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="values">
    /// A collection of <see cref="SqlValue"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "in" comparison between
    /// the value of the specified <paramref name="column"/> and the specified collection of
    /// <paramref name="values"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="values"/> argument, it
    /// will be automatically converted to an empty collection.
    /// </remarks>
    public static SqlBinaryExpression In(SqlColumn column, params SqlValue[] values)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.In,
            new SqlValueList(values ?? Enumerable.Empty<SqlValue>())
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an "in" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="query">
    /// A <see cref="SqlSubquery"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents an "in" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="query"/>.
    /// </returns>
    public static SqlBinaryExpression In(SqlColumn column, SqlSubquery query)
    {
        return new SqlBinaryExpression(column, SqlBinaryOperator.In, query);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a "not in" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="values">
    /// An array of <see cref="SqlValue"/> objects to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents a "not in" comparison between
    /// the value of the specified <paramref name="column"/> and the specified collection of
    /// <paramref name="values"/>.
    /// </returns>
    /// <remarks>
    /// If you pass <see langword="null"/> for the <paramref name="values"/> argument, it
    /// will be automatically converted to an empty collection.
    /// </remarks>
    public static SqlBinaryExpression NotIn(SqlColumn column, params SqlValue[] values)
    {
        return new SqlBinaryExpression(
            column,
            SqlBinaryOperator.NotIn,
            new SqlValueList(values ?? Enumerable.Empty<SqlValue>())
        );
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a "not in" comparison.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to use as the left operand in the expression.
    /// </param>
    /// <param name="query">
    /// A <see cref="SqlSubquery"/> to use as the right operand in the expression.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> that represents a "not in" comparison between
    /// the value of the specified <paramref name="column"/> and the specified <paramref name="query"/>.
    /// </returns>
    public static SqlBinaryExpression NotIn(SqlColumn column, SqlSubquery query)
    {
        return new SqlBinaryExpression(column, SqlBinaryOperator.NotIn, query);
    }

    /// <summary>
    /// Creates a <see cref="SqlBetween"/> that represents a BETWEEN expression.
    /// </summary>
    /// <param name="column">
    /// A <see cref="SqlColumn"/> to test.
    /// </param>
    /// <param name="min">
    /// The minimum value in the range.
    /// </param>
    /// <param name="max">
    /// The maximum value in the range.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBetween"/> that represents a BETWEEN comparison testing whether
    /// the value of the specified <paramref name="column"/> is between <paramref name="min"/> and <paramref name="max"/>.
    /// </returns>
    public static SqlBetween Between(SqlColumn column, SqlValue min, SqlValue max)
    {
        return new SqlBetween(column, min, max);
    }

    /// <summary>
    /// Creates a <see cref="SqlBetween"/> that represents a BETWEEN expression.
    /// </summary>
    /// <param name="function">
    /// A <see cref="SqlFunction"/> to test (typically used in HAVING clauses).
    /// </param>
    /// <param name="min">
    /// The minimum value in the range.
    /// </param>
    /// <param name="max">
    /// The maximum value in the range.
    /// </param>
    /// <returns>
    /// A <see cref="SqlBetween"/> that represents a BETWEEN comparison testing whether
    /// the result of the specified <paramref name="function"/> is between <paramref name="min"/> and <paramref name="max"/>.
    /// </returns>
    public static SqlBetween Between(SqlFunction function, SqlValue min, SqlValue max)
    {
        return new SqlBetween(function, min, max);
    }

    /// <summary>
    /// Creates a <see cref="SqlExists"/> that represents an EXISTS expression.
    /// </summary>
    /// <param name="subquery">
    /// The subquery to test for existence.
    /// </param>
    /// <returns>
    /// A <see cref="SqlExists"/> that represents an EXISTS expression testing whether
    /// the specified <paramref name="subquery"/> returns any rows.
    /// </returns>
    public static SqlExists Exists(SqlSubquery subquery)
    {
        return new SqlExists(subquery, isNegated: false);
    }

    /// <summary>
    /// Creates a <see cref="SqlExists"/> that represents a NOT EXISTS expression.
    /// </summary>
    /// <param name="subquery">
    /// The subquery to test for non-existence.
    /// </param>
    /// <returns>
    /// A <see cref="SqlExists"/> that represents a NOT EXISTS expression testing whether
    /// the specified <paramref name="subquery"/> returns no rows.
    /// </returns>
    public static SqlExists NotExists(SqlSubquery subquery)
    {
        return new SqlExists(subquery, isNegated: true);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents an addition (<c>+</c>) arithmetic operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>A <see cref="SqlBinaryExpression"/> that represents <c>left + right</c>.</returns>
    public static SqlBinaryExpression Add(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Add, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a subtraction (<c>-</c>) arithmetic operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>A <see cref="SqlBinaryExpression"/> that represents <c>left - right</c>.</returns>
    public static SqlBinaryExpression Subtract(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Subtract, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a multiplication (<c>*</c>) arithmetic operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>A <see cref="SqlBinaryExpression"/> that represents <c>left * right</c>.</returns>
    public static SqlBinaryExpression Multiply(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Multiply, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a division (<c>/</c>) arithmetic operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>A <see cref="SqlBinaryExpression"/> that represents <c>left / right</c>.</returns>
    public static SqlBinaryExpression Divide(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Divide, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a modulo (<c>%</c>) arithmetic operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>A <see cref="SqlBinaryExpression"/> that represents <c>left % right</c>.</returns>
    public static SqlBinaryExpression Modulo(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Modulo, right);
    }

    /// <summary>
    /// Creates a <see cref="SqlBinaryExpression"/> that represents a string concatenation operation.
    /// </summary>
    /// <param name="left">A <see cref="SqlValue"/> to use as the left operand.</param>
    /// <param name="right">A <see cref="SqlValue"/> to use as the right operand.</param>
    /// <returns>
    /// A <see cref="SqlBinaryExpression"/> with operator <see cref="SqlBinaryOperator.Concat"/>.
    /// The rendered SQL is dialect-specific: <c>||</c> (standard SQL / PostgreSQL),
    /// <c>+</c> (SQL Server), or <c>CONCAT(left, right)</c> (MySQL).
    /// </returns>
    public static SqlBinaryExpression Concat(SqlValue left, SqlValue right)
    {
        return new SqlBinaryExpression(left, SqlBinaryOperator.Concat, right);
    }

    /// <summary>
    /// Gets the expression type of this expression.
    /// </summary>
    public abstract SqlExpressionType ExpressionType { get; }

    /// <summary>
    /// Accepts the specified <paramref name="visitor"/> and dispatches calls to the specific visitor
    /// methods for this object.
    /// </summary>
    /// <param name="visitor">
    /// The <see cref="ISqlVisitor" /> to visit this object with.
    /// </param>
    public virtual void Accept(ISqlVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}
