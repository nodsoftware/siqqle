namespace Siqqle.Expressions;

/// <summary>
/// Describes the expression type for the nodes of a SQL expression tree.
/// </summary>
public enum SqlExpressionType
{
    /// <summary>
    /// Represents a SQL SELECT statement.
    /// </summary>
    Select,

    /// <summary>
    /// Represents a SQL DELETE statement.
    /// </summary>
    Delete,

    /// <summary>
    /// Represents a SQL INSERT statement.
    /// </summary>
    Insert,

    /// <summary>
    /// Represents a SQL UPDATE statement.
    /// </summary>
    Update,

    /// <summary>
    /// Represents a SQL identifier.
    /// </summary>
    Identifier,

    /// <summary>
    /// Represents a table in SQL.
    /// </summary>
    Column,

    /// <summary>
    /// Represents a table in SQL.
    /// </summary>
    Table,

    /// <summary>
    /// Represents a constant value in SQL.
    /// </summary>
    Constant,

    /// <summary>
    /// Represents a named parameter in SQL.
    /// </summary>
    Parameter,

    /// <summary>
    /// Represents a unary expression in SQL.
    /// </summary>
    Unary,

    /// <summary>
    /// Represents a binary expression in SQL.
    /// </summary>
    Binary,

    /// <summary>
    /// Represents a sorting expression in SQL.
    /// </summary>
    Sort,

    /// <summary>
    /// Represents a joining expression expression in SQL.
    /// </summary>
    Join,

    /// <summary>
    /// Represents a function call in SQL.
    /// </summary>
    Function,

    /// <summary>
    /// Represents a SQL FROM clause.
    /// </summary>
    From,

    /// <summary>
    /// Represents an assignment expression in SQL.
    /// </summary>
    Assign,

    /// <summary>
    /// Represents a SQL WHERE clause.
    /// </summary>
    Where,

    /// <summary>
    /// Represents a SQL ORDER BY clause.
    /// </summary>
    OrderBy,

    /// <summary>
    /// Represents a UNION operator in SQL.
    /// </summary>
    Union,

    /// <summary>
    /// Represents a SQL subquery in SQL.
    /// </summary>
    Subquery,

    /// <summary>
    /// Represents a SQL JOIN ON clause.
    /// </summary>
    On,

    /// <summary>
    /// Represents a SQL INSERT INTO clause.
    /// </summary>
    Into,

    /// <summary>
    /// Represents a SQL UPDATE SET clause.
    /// </summary>
    Set,

    /// <summary>
    /// Represents a SQL INSERT VALUES clause.
    /// </summary>
    Values,

    /// <summary>
    /// Represents a SQL expression list.
    /// </summary>
    ValueList,

    /// <summary>
    /// Represents a SQL GROUP BY clause.
    /// </summary>
    GroupBy,

    /// <summary>
    /// Represents a SQL GROUP BY HAVING clause.
    /// </summary>
    Having,

    /// <summary>
    /// Represents a SQL LIMIT clause.
    /// </summary>
    Limit,

    /// <summary>
    /// Represents a SQL data type definition.
    /// </summary>
    DataType,

    /// <summary>
    /// Represents a SQL CAST expression.
    /// </summary>
    Cast,

    /// <summary>
    /// Represents a SQL CASE expression.
    /// </summary>
    Case,

    /// <summary>
    /// Represents a SQL WHEN clause.
    /// </summary>
    When,

    /// <summary>
    /// Represents a SQL ELSE clause.
    /// </summary>
    Else,

    /// <summary>
    /// Represents a batch of SQL statements.
    /// </summary>
    Batch,

    /// <summary>
    /// Represents a SQL CALL statement.
    /// </summary>
    Call,

    /// <summary>
    /// Represents a SQL BETWEEN expression.
    /// </summary>
    Between,

    /// <summary>
    /// Represents a SQL EXISTS expression.
    /// </summary>
    Exists,
}
