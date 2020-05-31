using Dapper;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper
{
    internal static class CommandTextFactory
    {
        public static (string, object) Create<TStatement>(ISqlSyntaxEnd<TStatement> sql, object param = null)
            where TStatement : SqlStatement
        {
            return Create(sql?.Go(), param);
        }

        public static (string, object) Create<TStatement>(TStatement sql, object param = null)
            where TStatement : SqlStatement
        {
            var parameters = new DynamicParameters(param);
            var commandText = sql
                .ToSql(
                    parameter =>
                    {
                        parameters.Add(parameter.ParameterName, value: parameter.Value, dbType: parameter.DbType);
                    }
                );
            return (commandText, parameters);
        }
    }
}
