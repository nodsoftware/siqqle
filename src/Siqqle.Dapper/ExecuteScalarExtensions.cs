using Dapper;
using Siqqle.Expressions;
using Siqqle.Syntax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Siqqle.Dapper
{
    public static class ExecuteScalarExtensions
    {
        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <param name="cnn">The connection to execute on.</param>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="transaction">The transaction to use for this command.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell selected as <see cref="object"/>.</returns>
        public static object ExecuteScalar<TStatement>(this IDbConnection cnn, ISqlSyntaxEnd<TStatement> sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.ExecuteScalar<object>(command);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <param name="cnn">The connection to execute on.</param>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="transaction">The transaction to use for this command.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell selected as <see cref="object"/>.</returns>
        public static object ExecuteScalar<TStatement>(this IDbConnection cnn, TStatement sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.ExecuteScalar<object>(command);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="cnn">The connection to execute on.</param>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="transaction">The transaction to use for this command.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
        public static T ExecuteScalar<TStatement, T>(this IDbConnection cnn, ISqlSyntaxEnd<TStatement> sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.ExecuteScalar<T>(command);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="cnn">The connection to execute on.</param>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="param">The parameters to use for this command.</param>
        /// <param name="transaction">The transaction to use for this command.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
        public static T ExecuteScalar<TStatement, T>(this IDbConnection cnn, TStatement sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TStatement : SqlStatement
        {
            var command = CommandDefinitionFactory.Create(sql, param, transaction, commandTimeout, commandType);
            return cnn.ExecuteScalar<T>(command);
        }
    }
}
