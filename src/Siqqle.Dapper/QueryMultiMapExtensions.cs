using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Siqqle.Dialects;
using Siqqle.Expressions;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;

namespace Siqqle.Dapper;

public static class QueryMultiMapExtensions
{
    /// <summary>
    /// Perform a multi-mapping query with 2 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 2 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 3 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 3 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TThird, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 4 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 4 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 5 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 5 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 6 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<
        TFirst,
        TSecond,
        TThird,
        TFourth,
        TFifth,
        TSixth,
        TReturn
    >(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 6 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<
        TFirst,
        TSecond,
        TThird,
        TFourth,
        TFifth,
        TSixth,
        TReturn
    >(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 7 input types. If you need more types -> use Query with Type[] parameter.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<
        TFirst,
        TSecond,
        TThird,
        TFourth,
        TFifth,
        TSixth,
        TSeventh,
        TReturn
    >(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with 7 input types. If you need more types -> use Query with Type[] parameter.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<
        TFirst,
        TSecond,
        TThird,
        TFourth,
        TFifth,
        TSixth,
        TSeventh,
        TReturn
    >(
        this IDbConnection cnn,
        SqlSelect sql,
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with an arbitrary number of input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TReturn>(
        this IDbConnection cnn,
        ISqlSyntaxEnd<SqlSelect> sql,
        Type[] types,
        Func<object[], TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        return cnn.Query(
            sql?.Go(),
            types,
            map,
            param,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType,
            dialect
        );
    }

    /// <summary>
    /// Perform a multi-mapping query with an arbitrary number of input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="cnn">The connection to query on.</param>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="dialect">The SQL dialect to use. When <c>null</c>, uses the default dialect.</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    public static IEnumerable<TReturn> Query<TReturn>(
        this IDbConnection cnn,
        SqlSelect sql,
        Type[] types,
        Func<object[], TReturn> map,
        object param = null,
        IDbTransaction transaction = null,
        bool buffered = true,
        string splitOn = "Id",
        int? commandTimeout = null,
        CommandType? commandType = null,
        SqlDialect dialect = null
    )
    {
        (var commandText, var parameters) = CommandTextFactory.Create(sql, param, dialect);
        return cnn.Query(
            commandText,
            types,
            map,
            parameters,
            transaction,
            buffered,
            splitOn,
            commandTimeout,
            commandType
        );
    }
}
