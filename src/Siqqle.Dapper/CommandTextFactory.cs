using System;
using System.Data;
using Dapper;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

internal static class CommandTextFactory
{
    public static (string, object) Create<TStatement>(
        ISqlSyntaxEnd<TStatement> sql,
        object param = null
    )
        where TStatement : SqlStatement
    {
        return Create(sql?.Go(), param);
    }

    public static (string, object) Create<TStatement>(TStatement sql, object param = null)
        where TStatement : SqlStatement
    {
        var parameters = new DynamicParameters(param);
        var commandText = sql.ToSql(parameter =>
        {
            var value = parameter.Value;
            var dbType = parameter.DbType;

            // Dapper does not natively support DateOnly; convert to DateTime with DbType.Date
            if (value is DateOnly dateOnly)
            {
                value = dateOnly.ToDateTime(TimeOnly.MinValue);
                dbType ??= DbType.Date;
            }

            parameters.Add(
                parameter.ParameterName,
                value: value,
                dbType: dbType
            );
        });
        return (commandText, parameters);
    }
}
