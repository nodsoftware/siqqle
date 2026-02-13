using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithIsNull_ReturnsSqlWithIsNull()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Email] IS NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNull(new SqlColumn("Email")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNull_ReturnsSqlWithIsNotNull()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Email] IS NOT NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNotNull(new SqlColumn("Email")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNullInJoinCondition_ReturnsSqlWithIsNull()
    {
        const string expected =
            "SELECT [u].[Name] FROM [Users] [u] JOIN [Orders] [o] ON [u].[Id] = [o].[UserId] WHERE [o].[ShippedDate] IS NULL";

        SqlTable users = new("Users", "u");
        SqlTable orders = new("Orders", "o");

        var actual = Sql.Select(users + "Name")
            .From(users)
            .Join(orders)
            .On(SqlExpression.Equal(users + "Id", orders + "UserId"))
            .Where(SqlExpression.IsNull(orders + "ShippedDate"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNullInSubquery_ReturnsSqlWithIsNotNull()
    {
        const string expected =
            "SELECT [Name] FROM [Products] WHERE [CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [ParentId] IS NOT NULL)";

        var subquery = new SqlSubquery(
            Sql.Select("Id")
                .From("Categories")
                .Where(SqlExpression.IsNotNull(new SqlColumn("ParentId")))
        );

        var actual = Sql.Select("Name")
            .From("Products")
            .Where(SqlExpression.In("CategoryId", subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNullTableQualifiedColumn_ReturnsSqlWithIsNull()
    {
        const string expected =
            "SELECT [u].[Id], [u].[Name] FROM [Users] [u] WHERE [u].[DeletedAt] IS NULL";

        SqlTable users = new("Users", "u");

        var actual = Sql.Select(users + "Id", users + "Name")
            .From(users)
            .Where(SqlExpression.IsNull(users + "DeletedAt"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNullTableQualifiedColumn_ReturnsSqlWithIsNotNull()
    {
        const string expected =
            "SELECT [p].[Name] FROM [Products] [p] WHERE [p].[Description] IS NOT NULL";

        SqlTable products = new("Products", "p");

        var actual = Sql.Select(products + "Name")
            .From(products)
            .Where(SqlExpression.IsNotNull(products + "Description"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithMultipleIsNullConditions_ReturnsSqlWithIsNull()
    {
        const string expected =
            "SELECT [Name] FROM [Users] WHERE ([Email] IS NULL) AND ([Phone] IS NULL)";

        var actual = Sql.Select("Name")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.IsNull(new SqlColumn("Email")),
                    SqlExpression.IsNull(new SqlColumn("Phone"))
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNullAndIsNotNull_ReturnsSqlWithBoth()
    {
        const string expected =
            "SELECT [Name] FROM [Users] WHERE ([Email] IS NULL) AND ([Phone] IS NOT NULL)";

        var actual = Sql.Select("Name")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.IsNull(new SqlColumn("Email")),
                    SqlExpression.IsNotNull(new SqlColumn("Phone"))
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNullInOrderBy_ReturnsSqlWithIsNull()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE [DeletedAt] IS NULL ORDER BY [Name]";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNull(new SqlColumn("DeletedAt")))
            .OrderBy("Name")
            .ToSql();

        Assert.Equal(expected, actual);
    }
}
