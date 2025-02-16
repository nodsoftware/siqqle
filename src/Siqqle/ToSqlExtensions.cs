using System;
using System.Text;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;
using Siqqle.Text;

namespace Siqqle;

public static class ToSqlExtensions
{
    public static string ToSql<TStatement>(this TStatement statement)
        where TStatement : SqlStatement
    {
        return statement.ToSqlInternal();
    }

    public static string ToSql<TStatement>(this TStatement statement, SqlDialect dialect)
        where TStatement : SqlStatement
    {
        return statement.ToSqlInternal(dialect: dialect);
    }

    public static string ToSql<TStatement>(
        this TStatement statement,
        Action<SqlParameter> parameterCallback
    )
        where TStatement : SqlStatement
    {
        return statement.ToSqlInternal(parameterCallback: parameterCallback);
    }

    public static string ToSql<TStatement>(
        this TStatement statement,
        SqlDialect dialect,
        Action<SqlParameter> parameterCallback
    )
        where TStatement : SqlStatement
    {
        return statement.ToSqlInternal(dialect: dialect, parameterCallback: parameterCallback);
    }

    public static string ToSql<TStatement>(
        this ISqlSyntaxEnd<TStatement> syntax,
        Action<SqlParameter> parameterCallback
    )
        where TStatement : SqlStatement
    {
        var statement = syntax.Go();
        return statement.ToSqlInternal(parameterCallback: parameterCallback);
    }

    public static string ToSql<TStatement>(
        this ISqlSyntaxEnd<TStatement> syntax,
        SqlDialect dialect = null,
        Action<SqlParameter> parameterCallback = null
    )
        where TStatement : SqlStatement
    {
        var statement = syntax.Go();
        return statement.ToSqlInternal(dialect, parameterCallback);
    }

    internal static string ToSqlInternal(
        this SqlStatement sql,
        SqlDialect dialect = null,
        Action<SqlParameter> parameterCallback = null
    )
    {
        ArgumentNullException.ThrowIfNull(sql);
        dialect ??= SqlDialect.Current;

        StringBuilder builder = new();
        using (SqlWriter writer = new(builder, dialect))
        {
            var visitor = new SqlWriterVisitor(writer) { ParameterVisited = parameterCallback };

            sql.Accept(visitor);
        }

        return builder.ToString();
    }
}
