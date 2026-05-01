using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithInValues_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE [Status] IN ('Active', 'Pending', 'Approved')";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.In("Status", "Active", "Pending", "Approved"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInNumericValues_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [ProductName] FROM [Products] WHERE [CategoryId] IN (1, 2, 3, 5)";

        var actual = Sql.Select("ProductName")
            .From("Products")
            .Where(SqlExpression.In("CategoryId", 1, 2, 3, 5))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotInValues_ReturnsSqlWithNotIn()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE [Status] NOT IN ('Deleted', 'Suspended', 'Banned')";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotIn("Status", "Deleted", "Suspended", "Banned"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInSubquery_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [Name] FROM [Products] WHERE [CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [IsActive] = 1)";

        var subquery = new SqlSubquery(
            Sql.Select("Id").From("Categories").Where(SqlExpression.Equal("IsActive", 1))
        );

        var actual = Sql.Select("Name")
            .From("Products")
            .Where(SqlExpression.In("CategoryId", subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotInSubquery_ReturnsSqlWithNotIn()
    {
        const string expected =
            "SELECT [Name] FROM [Users] WHERE [Id] NOT IN (SELECT [UserId] FROM [BlockedUsers])";

        var subquery = new SqlSubquery(Sql.Select("UserId").From("BlockedUsers"));

        var actual = Sql.Select("Name")
            .From("Users")
            .Where(SqlExpression.NotIn("Id", subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInAndOtherConditions_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE ([Status] IN ('Active', 'Pending')) AND ([Age] > 18)";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.In("Status", "Active", "Pending"),
                    SqlExpression.GreaterThan("Age", 18)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotInAndOtherConditions_ReturnsSqlWithNotIn()
    {
        const string expected =
            "SELECT [ProductId] FROM [Products] WHERE ([CategoryId] NOT IN (10, 20, 30)) AND ([Price] < 100)";

        var actual = Sql.Select("ProductId")
            .From("Products")
            .Where(
                SqlExpression.And(
                    SqlExpression.NotIn("CategoryId", 10, 20, 30),
                    SqlExpression.LessThan("Price", 100)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInOrCondition_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [Id] FROM [Orders] WHERE ([Status] = 'Pending') OR ([Priority] IN (1, 2))";

        var actual = Sql.Select("Id")
            .From("Orders")
            .Where(
                SqlExpression.Or(
                    SqlExpression.Equal("Status", "Pending"),
                    SqlExpression.In("Priority", 1, 2)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithMultipleInConditions_ReturnsSqlWithMultipleIn()
    {
        const string expected =
            "SELECT [Id] FROM [Products] WHERE ([CategoryId] IN (1, 2, 3)) AND ([BrandId] IN (10, 20))";

        var actual = Sql.Select("Id")
            .From("Products")
            .Where(
                SqlExpression.And(
                    SqlExpression.In("CategoryId", 1, 2, 3),
                    SqlExpression.In("BrandId", 10, 20)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInSingleValue_ReturnsSqlWithIn()
    {
        const string expected = "SELECT [Name] FROM [Users] WHERE [Status] IN ('Active')";

        var actual = Sql.Select("Name")
            .From("Users")
            .Where(SqlExpression.In("Status", "Active"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInMixedTypes_ReturnsSqlWithIn()
    {
        const string expected = "SELECT [Id] FROM [Data] WHERE [Value] IN (1, 'test', 3.14)";

        var actual = Sql.Select("Id")
            .From("Data")
            .Where(SqlExpression.In("Value", 1, "test", 3.14))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInCorrelatedSubquery_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [DepartmentName] FROM [Departments] WHERE [Id] IN (SELECT [DepartmentId] FROM [Employees] WHERE [Employees].[Salary] > 50000)";

        var subquery = new SqlSubquery(
            Sql.Select("DepartmentId")
                .From("Employees")
                .Where(SqlExpression.GreaterThan(new SqlColumn("Employees.Salary"), 50000))
        );

        var actual = Sql.Select("DepartmentName")
            .From("Departments")
            .Where(SqlExpression.In("Id", subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInParameters_ReturnsSqlWithIn()
    {
        const string expected = "SELECT [Name] FROM [Users] WHERE [Id] IN (@Id1, @Id2, @Id3)";

        var actual = Sql.Select("Name")
            .From("Users")
            .Where(
                SqlExpression.In(
                    new SqlColumn("Id"),
                    new SqlParameter("Id1", 1),
                    new SqlParameter("Id2", 2),
                    new SqlParameter("Id3", 3)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithInTableQualifiedColumn_ReturnsSqlWithIn()
    {
        const string expected =
            "SELECT [u].[Name] FROM [Users] [u] WHERE [u].[Status] IN ('Active', 'Pending')";

        SqlTable users = new("Users", "u");

        var actual = Sql.Select(users + "Name")
            .From(users)
            .Where(SqlExpression.In(users + "Status", "Active", "Pending"))
            .ToSql();

        Assert.Equal(expected, actual);
    }
}
