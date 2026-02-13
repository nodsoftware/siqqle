using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithLike_ReturnsSqlWithLike()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Name] LIKE 'John%'";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.Like(new SqlColumn("Name"), new SqlConstant("John%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotLike_ReturnsSqlWithNotLike()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Name] NOT LIKE '%Admin%'";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotLike(new SqlColumn("Name"), new SqlConstant("%Admin%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLikePrefix_ReturnsSqlWithLike()
    {
        const string expected = "SELECT [Email] FROM [Users] WHERE [Email] LIKE 'john%'";

        var actual = Sql.Select("Email")
            .From("Users")
            .Where(SqlExpression.Like(new SqlColumn("Email"), new SqlConstant("john%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLikeSuffix_ReturnsSqlWithLike()
    {
        const string expected = "SELECT [Email] FROM [Users] WHERE [Email] LIKE '%@example.com'";

        var actual = Sql.Select("Email")
            .From("Users")
            .Where(SqlExpression.Like(new SqlColumn("Email"), new SqlConstant("%@example.com")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLikeContains_ReturnsSqlWithLike()
    {
        const string expected =
            "SELECT [Description] FROM [Products] WHERE [Description] LIKE '%smartphone%'";

        var actual = Sql.Select("Description")
            .From("Products")
            .Where(
                SqlExpression.Like(new SqlColumn("Description"), new SqlConstant("%smartphone%"))
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotLikePattern_ReturnsSqlWithNotLike()
    {
        const string expected =
            "SELECT [ProductName] FROM [Products] WHERE [ProductName] NOT LIKE 'Test%'";

        var actual = Sql.Select("ProductName")
            .From("Products")
            .Where(SqlExpression.NotLike(new SqlColumn("ProductName"), new SqlConstant("Test%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLikeEscape_ReturnsSqlWithLike()
    {
        const string expected = "SELECT [Name] FROM [Files] WHERE [Name] LIKE '50\\%'";

        var actual = Sql.Select("Name")
            .From("Files")
            .Where(SqlExpression.Like(new SqlColumn("Name"), new SqlConstant("50\\%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLikeTableQualifiedColumn_ReturnsSqlWithLike()
    {
        const string expected =
            "SELECT [u].[Id], [u].[Name] FROM [Users] [u] WHERE [u].[Name] LIKE 'A%'";

        SqlTable users = new("Users", "u");

        var actual = Sql.Select(users + "Id", users + "Name")
            .From(users)
            .Where(SqlExpression.Like(users + "Name", new SqlConstant("A%")))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotLikeTableQualifiedColumn_ReturnsSqlWithNotLike()
    {
        const string expected =
            "SELECT [u].[Id], [u].[Email] FROM [Users] [u] WHERE [u].[Email] NOT LIKE '%@spam.com'";

        SqlTable users = new("Users", "u");

        var actual = Sql.Select(users + "Id", users + "Email")
            .From(users)
            .Where(SqlExpression.NotLike(users + "Email", new SqlConstant("%@spam.com")))
            .ToSql();

        Assert.Equal(expected, actual);
    }
}
