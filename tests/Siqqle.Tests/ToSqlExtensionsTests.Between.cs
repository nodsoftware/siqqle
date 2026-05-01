using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithBetween_ReturnsSqlWithBetween()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Age] BETWEEN 18 AND 65";
        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.Between("Age", 18, 65))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenDecimals_ReturnsSqlWithBetween()
    {
        const string expected =
            "SELECT [ProductName] FROM [Products] WHERE [Price] BETWEEN 10.99 AND 99.99";
        var actual = Sql.Select("ProductName")
            .From("Products")
            .Where(SqlExpression.Between("Price", 10.99m, 99.99m))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenDates_ReturnsSqlWithBetween()
    {
        var startDate = new System.DateTime(2024, 1, 1);
        var endDate = new System.DateTime(2024, 12, 31);

        var actual = Sql.Select("OrderId")
            .From("Orders")
            .Where(SqlExpression.Between("OrderDate", startDate, endDate))
            .ToSql();

        // Verify the structure is correct (format may vary by culture)
        Assert.Contains("SELECT [OrderId] FROM [Orders] WHERE [OrderDate] BETWEEN", actual);
        Assert.Contains("AND", actual);
    }

    [Fact]
    public void ToSql_WithBetweenParameters_ReturnsSqlWithBetween()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE [Age] BETWEEN @MinAge AND @MaxAge";
        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(
                SqlExpression.Between("Age", "MinAge" + (SqlConstant)18, "MaxAge" + (SqlConstant)65)
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenAndOtherConditions_ReturnsSqlWithBetween()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE ([Age] BETWEEN 18 AND 65) AND ([Status] = 'Active')";
        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.Between("Age", 18, 65),
                    SqlExpression.Equal("Status", "Active")
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenInHaving_ReturnsSqlWithBetween()
    {
        SqlTable products = new("Products", "p");

        const string expected =
            "SELECT [p].[Category], AVG([p].[Price]) AS [AvgPrice] FROM [Products] [p] GROUP BY ([p].[Category]) HAVING AVG([p].[Price]) BETWEEN 50 AND 200";
        var actual = Sql.Select(
                products + "Category",
                SqlAggregate.Average(products + "Price", "AvgPrice")
            )
            .From(products)
            .GroupBy(products + "Category")
            .Having(SqlExpression.Between(SqlAggregate.Average(products + "Price"), 50, 200))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenNegativeNumbers_ReturnsSqlWithBetween()
    {
        const string expected =
            "SELECT [Temperature] FROM [Readings] WHERE [Value] BETWEEN -10 AND 10";
        var actual = Sql.Select("Temperature")
            .From("Readings")
            .Where(SqlExpression.Between("Value", -10, 10))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithMultipleBetween_ReturnsSqlWithMultipleBetween()
    {
        const string expected =
            "SELECT [Name] FROM [Employees] WHERE ([Age] BETWEEN 25 AND 45) AND ([Salary] BETWEEN 30000 AND 80000)";
        var actual = Sql.Select("Name")
            .From("Employees")
            .Where(
                SqlExpression.And(
                    SqlExpression.Between("Age", 25, 45),
                    SqlExpression.Between("Salary", 30000, 80000)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenOrCondition_ReturnsSqlWithBetween()
    {
        const string expected =
            "SELECT [Name] FROM [Products] WHERE ([Price] BETWEEN 10 AND 50) OR ([Price] BETWEEN 100 AND 200)";
        var actual = Sql.Select("Name")
            .From("Products")
            .Where(
                SqlExpression.Or(
                    SqlExpression.Between("Price", 10, 50),
                    SqlExpression.Between("Price", 100, 200)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithBetweenTableAlias_ReturnsSqlWithBetween()
    {
        SqlTable users = new("Users", "u");

        const string expected =
            "SELECT [u].[Id], [u].[Name] FROM [Users] [u] WHERE [u].[Age] BETWEEN 18 AND 65";
        var actual = Sql.Select(users + "Id", users + "Name")
            .From(users)
            .Where(SqlExpression.Between(users + "Age", 18, 65))
            .ToSql();

        Assert.Equal(expected, actual);
    }
}
