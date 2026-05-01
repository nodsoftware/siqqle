namespace Siqqle.Expressions;

/// <summary>
/// Represents all available binary operators in SQL.
/// </summary>
public enum SqlBinaryOperator
{
    /// <summary>
    /// Represents the logical <b>AND</b> operator.
    /// </summary>
    And,

    /// <summary>
    /// Represents the logical <b>OR</b> operator.
    /// </summary>
    Or,

    /// <summary>
    /// Represents the equality comparison operator (<c>=</c>).
    /// </summary>
    Equal,

    /// <summary>
    /// Represents the inequality comparison operator (<c>&lt;&gt;</c>).
    /// </summary>
    NotEqual,

    /// <summary>
    /// Represents the "greater than" comparison operator (<c>&gt;</c>).
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Represents the "greater than or equal to" comparison operator (<c>&gt;=</c>).
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Represents the "less than" comparison operator (<c>&lt;</c>).
    /// </summary>
    LessThan,

    /// <summary>
    /// Represents the "less than or equal to" comparison operator (<c>&lt;</c>).
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Represents the "like" comparison operator (<c>LIKE</c>).
    /// </summary>
    Like,

    /// <summary>
    /// Represents the "not like" comparison operator (<c>NOT LIKE</c>).
    /// </summary>
    NotLike,

    /// <summary>
    /// Represents the "in" comparison operator (<c>IN</c>).
    /// </summary>
    In,

    /// <summary>
    /// Represents the "not in" comparison operator (<c>NOT IN</c>).
    /// </summary>
    NotIn,

    /// <summary>
    /// Represents the addition arithmetic operator (<c>+</c>).
    /// </summary>
    Add,

    /// <summary>
    /// Represents the subtraction arithmetic operator (<c>-</c>).
    /// </summary>
    Subtract,

    /// <summary>
    /// Represents the multiplication arithmetic operator (<c>*</c>).
    /// </summary>
    Multiply,

    /// <summary>
    /// Represents the division arithmetic operator (<c>/</c>).
    /// </summary>
    Divide,

    /// <summary>
    /// Represents the modulo arithmetic operator (<c>%</c>).
    /// </summary>
    Modulo,

    /// <summary>
    /// Represents the string concatenation operator.
    /// </summary>
    /// <remarks>
    /// The rendered SQL is dialect-specific: <c>||</c> (standard SQL / PostgreSQL),
    /// <c>+</c> (SQL Server), or <c>CONCAT(left, right)</c> (MySQL).
    /// Use this operator for string concatenation. For numeric addition, use <see cref="Add"/>.
    /// </remarks>
    Concat,
}
