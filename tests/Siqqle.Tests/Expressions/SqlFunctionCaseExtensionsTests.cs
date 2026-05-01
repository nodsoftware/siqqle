using System;
using System.Linq;
using Siqqle.Dialects;
using Siqqle.Dialects.MySql;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlFunctionCaseExtensionsTests
{
    [Fact]
    public void Lower_WithColumn_ReturnsFunctionWithCorrectName()
    {
        var column = (SqlColumn)"Name";

        var result = SqlFunction.Lower(column);

        Assert.IsType<SqlFunction>(result);
        Assert.Equal("LOWER", result.FunctionName);
    }

    [Fact]
    public void Lower_WithColumn_IncludesColumnAsArgument()
    {
        var column = (SqlColumn)"Name";

        var result = SqlFunction.Lower(column);

        Assert.NotNull(result.Arguments);
        var args = result.Arguments.ToList();
        Assert.Single(args);
        Assert.Same(column, args[0]);
    }

    [Fact]
    public void Lower_WithParameter_ReturnsFunctionWithCorrectName()
    {
        var parameter = new SqlParameter("Name", "value");

        var result = SqlFunction.Lower(parameter);

        Assert.IsType<SqlFunction>(result);
        Assert.Equal("LOWER", result.FunctionName);
    }

    [Fact]
    public void Lower_WithConstant_ReturnsFunctionWithCorrectName()
    {
        var constant = new SqlConstant("test");

        var result = SqlFunction.Lower(constant);

        Assert.IsType<SqlFunction>(result);
        Assert.Equal("LOWER", result.FunctionName);
    }

    [Fact]
    public void Upper_WithColumn_ReturnsFunctionWithCorrectName()
    {
        var column = (SqlColumn)"Name";

        var result = SqlFunction.Upper(column);

        Assert.IsType<SqlFunction>(result);
        Assert.Equal("UPPER", result.FunctionName);
    }

    [Fact]
    public void Upper_WithParameter_ReturnsFunctionWithCorrectName()
    {
        var parameter = new SqlParameter("Name", "value");

        var result = SqlFunction.Upper(parameter);

        Assert.IsType<SqlFunction>(result);
        Assert.Equal("UPPER", result.FunctionName);
    }

    [Fact]
    public void Lower_WithNull_ThrowsArgumentNull()
    {
        SqlValue nullValue = null;

        Assert.Throws<ArgumentNullException>(() => SqlFunction.Lower(nullValue));
    }

    [Fact]
    public void Upper_WithNull_ThrowsArgumentNull()
    {
        SqlValue nullValue = null;

        Assert.Throws<ArgumentNullException>(() => SqlFunction.Upper(nullValue));
    }

    [Fact]
    public void Lower_WithNestedFunction_ReturnsNestedLowerFunctions()
    {
        var column = (SqlColumn)"Name";
        var lower = SqlFunction.Lower(column);

        var nested = SqlFunction.Lower(lower);

        Assert.Equal("LOWER", nested.FunctionName);
        var args = nested.Arguments.ToList();
        Assert.Single(args);
        Assert.Same(lower, args[0]);
    }

    [Fact]
    public void NestedFunctions_LowerThenUpper_ReturnsCorrectStructure()
    {
        var column = (SqlColumn)"Name";
        var lower = SqlFunction.Lower(column);
        var upper = SqlFunction.Upper(lower);

        Assert.Equal("UPPER", upper.FunctionName);
        var args = upper.Arguments.ToList();
        Assert.Single(args);
        Assert.Same(lower, args[0]);
    }

    [Fact]
    public void Equal_WithTwoLowerFunctions_CreatesCorrectExpression()
    {
        var column = (SqlColumn)"UserName";
        var parameter = new SqlParameter("UserInput", "value");

        var lowerColumn = SqlFunction.Lower(column);
        var lowerParameter = SqlFunction.Lower(parameter);
        var expression = SqlExpression.Equal(lowerColumn, lowerParameter);

        Assert.IsType<SqlBinaryExpression>(expression);
        Assert.Equal(SqlBinaryOperator.Equal, expression.Operator);
        Assert.Same(lowerColumn, expression.Left);
        Assert.Same(lowerParameter, expression.Right);
    }

    [Fact]
    public void Equal_WithMixedCaseInsensitiveFunctions_WorksCorrectly()
    {
        var column = (SqlColumn)"Email";
        var parameter = new SqlParameter("EmailInput", "test@example.com");

        var lowerColumn = SqlFunction.Lower(column);
        var upperParameter = SqlFunction.Upper(parameter);
        var expression = SqlExpression.Equal(lowerColumn, upperParameter);

        Assert.IsType<SqlBinaryExpression>(expression);
        Assert.Equal(SqlBinaryOperator.Equal, expression.Operator);
    }
}

public class SqlFunctionCaseExtensionsDialectTests
{
    [Fact]
    public void Lower_WithColumn_RendersCorrectlyInSqlServer()
    {
        var column = (SqlColumn)"Name";
        var lower = SqlFunction.Lower(column);

        var select = Sql
            .Select(lower)
            .From("Users");

        var dialect = new SqlServerDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER([Name])", sql);
    }

    [Fact]
    public void Upper_WithColumn_RendersCorrectlyInPostgreSql()
    {
        var column = (SqlColumn)"Name";
        var upper = SqlFunction.Upper(column);

        var select = Sql
            .Select(upper)
            .From("Users");

        var dialect = new PostgreSqlDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("UPPER(\"Name\")", sql);
    }

    [Fact]
    public void Lower_WithColumn_RendersCorrectlyInMySql()
    {
        var column = (SqlColumn)"Name";
        var lower = SqlFunction.Lower(column);

        var select = Sql
            .Select(lower)
            .From("Users");

        var dialect = new MySqlDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER(`Name`)", sql);
    }

    [Fact]
    public void Equal_WithTwoLowerFunctions_RendersCorrectlyInSqlServer()
    {
        var column = (SqlColumn)"UserName";
        var parameter = new SqlParameter("UserInput", "TestUser");

        var lowerColumn = SqlFunction.Lower(column);
        var lowerParameter = SqlFunction.Lower(parameter);
        var whereExpression = SqlExpression.Equal(lowerColumn, lowerParameter);

        var select = Sql
            .Select((SqlColumn)"UserId")
            .From("Users")
            .Where(whereExpression);

        var dialect = new SqlServerDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER(", sql);
        Assert.Contains("[UserName]", sql);
        Assert.Contains("=", sql);
    }

    [Fact]
    public void Equal_WithTwoLowerFunctions_RendersCorrectlyInPostgreSql()
    {
        var column = (SqlColumn)"UserName";
        var parameter = new SqlParameter("UserInput", "TestUser");

        var lowerColumn = SqlFunction.Lower(column);
        var lowerParameter = SqlFunction.Lower(parameter);
        var whereExpression = SqlExpression.Equal(lowerColumn, lowerParameter);

        var select = Sql
            .Select((SqlColumn)"UserId")
            .From("Users")
            .Where(whereExpression);

        var dialect = new PostgreSqlDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER(", sql);
        Assert.Contains("\"UserName\"", sql);
        Assert.Contains("=", sql);
    }

    [Fact]
    public void Equal_WithTwoLowerFunctions_RendersCorrectlyInMySql()
    {
        var column = (SqlColumn)"UserName";
        var parameter = new SqlParameter("UserInput", "TestUser");

        var lowerColumn = SqlFunction.Lower(column);
        var lowerParameter = SqlFunction.Lower(parameter);
        var whereExpression = SqlExpression.Equal(lowerColumn, lowerParameter);

        var select = Sql
            .Select((SqlColumn)"UserId")
            .From("Users")
            .Where(whereExpression);

        var dialect = new MySqlDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER(", sql);
        Assert.Contains("`UserName`", sql);
        Assert.Contains("=", sql);
    }

    [Fact]
    public void NestedFunctions_LowerThenUpper_RendersCorrectly()
    {
        var column = (SqlColumn)"Name";
        var nested = SqlFunction.Upper(SqlFunction.Lower(column));

        var select = Sql
            .Select(nested)
            .From("Users");

        var dialect = new SqlServerDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("UPPER(LOWER([Name]))", sql);
    }

    [Fact]
    public void ComplexExpression_WithMultipleFunctions_RendersCorrectly()
    {
        var firstName = (SqlColumn)"FirstName";
        var lastName = (SqlColumn)"LastName";
        var searchInput = new SqlParameter("SearchName", "john");

        var whereExpression = SqlExpression.And(
            SqlExpression.Equal(SqlFunction.Lower(firstName), SqlFunction.Lower(searchInput)),
            SqlExpression.Equal(SqlFunction.Upper(lastName), SqlFunction.Upper(new SqlConstant("DOE")))
        );

        var select = Sql
            .Select((SqlColumn)"UserId")
            .From("Users")
            .Where(whereExpression);

        var dialect = new SqlServerDialect();
        var sql = select.ToSql(dialect);

        Assert.Contains("LOWER([FirstName]) = LOWER(@SearchName)", sql);
        Assert.Contains("UPPER([LastName]) = UPPER('DOE')", sql);
    }
}
