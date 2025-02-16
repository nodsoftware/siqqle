using System.Linq;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle;

/// <summary>
/// Provides <see langword="static" /> factory methods for building SQL expression trees.
/// </summary>
public static class Sql
{
    /// <summary>
    /// Creates a <see cref="SqlSelect"/> that selects the specified <paramref name="columns"/> from a table.
    /// </summary>
    /// <param name="columns">
    /// The columns to select from the table.
    /// </param>
    /// <returns>
    /// A <see cref="SqlSelect"/> that selects the specified <paramref name="columns"/> from a table.
    /// </returns>
    public static ISqlSelectSyntax Select(params SqlColumn[] columns)
    {
        return new SqlSelectBuilder(columns);
    }

    /// <summary>
    /// Creates a <see cref="SqlSelect"/> that selects the specified <paramref name="columns"/> from a table.
    /// </summary>
    /// <param name="columns">
    /// The columns to select from the table.
    /// </param>
    /// <returns>
    /// A <see cref="SqlSelect"/> that selects the specified <paramref name="columns"/> from a table.
    /// </returns>
    public static ISqlSelectSyntax Select(params SqlValue[] columns)
    {
        return new SqlSelectBuilder(columns);
    }

    /// <summary>
    /// Creates a <see cref="SqlUnion"/> that combines the specified <see cref="SqlSelect"/> <paramref name="statements"/>.
    /// </summary>
    /// <param name="statements"></param>
    /// <returns>
    /// A <see cref="SqlUnion"/> that combines the specified <see cref="SqlSelect"/> <paramref name="statements"/>.
    /// </returns>
    public static SqlUnion Union(params SqlSelect[] statements)
    {
        return new SqlUnion(statements);
    }

    /// <summary>
    /// Creates a <see cref="SqlUnion"/> that combines the specified <see cref="SqlSelect"/> <paramref name="statements"/>.
    /// </summary>
    /// <param name="statements"></param>
    /// <returns>
    /// A <see cref="SqlUnion"/> that combines the specified <see cref="SqlSelect"/> <paramref name="statements"/>.
    /// </returns>
    public static SqlUnion Union(params ISqlSyntaxEnd<SqlSelect>[] statements)
    {
        return new SqlUnion(statements.Where(_ => _ != null).Select(_ => _.Go()));
    }

    /// <summary>
    /// Creates a <see cref="SqlBatch"/> that concatenates the specified <see cref="SqlStatement"/> <paramref name="statements"/>.
    /// </summary>
    /// <param name="statements"></param>
    /// <returns>
    /// A <see cref="SqlBatch"/> that concatenates the specified <see cref="SqlStatement"/> <paramref name="statements"/>.
    /// </returns>
    public static SqlBatch Batch(params SqlStatement[] statements)
    {
        return new SqlBatch(statements);
    }

    /// <summary>
    /// Creates a <see cref="SqlBatch"/> that concatanates the specified <see cref="SqlStatement"/> <paramref name="statements"/>.
    /// </summary>
    /// <param name="statements"></param>
    /// <returns>
    /// A <see cref="SqlBatch"/> that concatenates the specified <see cref="SqlStatement"/> <paramref name="statements"/>.
    /// </returns>
    public static SqlBatch Batch(params ISqlSyntaxEnd<SqlStatement>[] statements)
    {
        return new SqlBatch(statements.Where(_ => _ != null).Select(_ => _.Go()));
    }

    /// <summary>
    /// Creates a <see cref="SqlCall"/> that calls a stored procedure with the specified <paramref name="procedureName"/> and <paramref name="arguments"/>.
    /// </summary>
    /// <param name="procedureName">
    /// The name of the stored procedure to call.
    /// </param>
    /// <param name="arguments">
    /// The arguments to pass to the stored procedure.
    /// </param>
    /// <returns>
    /// A <see cref="SqlCall"/> that calls a stored procedure with the specified <paramref name="procedureName"/> and <paramref name="arguments"/>.
    /// </returns>
    public static SqlCall Call(string procedureName, params SqlValue[] arguments)
    {
        return new SqlCall(procedureName, arguments);
    }

    /// <summary>
    /// Creates a <see cref="SqlDelete"/> that deletes rows from a table.
    /// </summary>
    /// <returns>
    /// A <see cref="SqlDelete"/> that deletes rows from a table.
    /// </returns>
    public static ISqlDeleteSyntax Delete()
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

    /// <summary>
    /// Creates a <see cref="SqlUpdate"/> that updates rows in the specified <paramref name="table"/>.
    /// </summary>
    /// <param name="table">
    /// The table to update.
    /// </param>
    /// <returns>
    /// A <see cref="SqlUpdate"/> that updates rows in the specified <paramref name="table"/>.
    /// </returns>
    public static ISqlUpdateSyntax Update(SqlTable table)
    {
        return new SqlUpdateBuilder(table);
    }
}
