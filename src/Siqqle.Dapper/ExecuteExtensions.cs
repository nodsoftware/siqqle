using Dapper;
using Siqqle.Expressions;
using Siqqle.Syntax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Siqqle.Dapper
{
    public static class ExecuteExtensions
    {
        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public static int Execute<TStatement>(this IDbConnection cnn, ISqlSyntaxEnd<TStatement> sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.Execute(command);
        }

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public static int Execute<TStatement>(this IDbConnection cnn, TStatement sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.Execute(command);
        }
    }
}
