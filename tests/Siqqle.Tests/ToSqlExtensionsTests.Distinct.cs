using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithDistinct_ReturnsSqlWithDistinct()
    {
        const string expected = "SELECT DISTINCT [City] FROM [Users]";
        var actual = Sql.Select("City").Distinct().From("Users").ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctMultipleColumns_ReturnsSqlWithDistinct()
    {
        const string expected = "SELECT DISTINCT [City], [Country] FROM [Users]";
        var actual = Sql.Select("City", "Country").Distinct().From("Users").ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctAndWhere_ReturnsSqlWithDistinct()
    {
        const string expected = "SELECT DISTINCT [City] FROM [Users] WHERE [Age] >= 18";
        var actual = Sql.Select("City")
            .Distinct()
            .From("Users")
            .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctAndOrderBy_ReturnsSqlWithDistinct()
    {
        const string expected = "SELECT DISTINCT [City] FROM [Users] ORDER BY [City]";
        var actual = Sql.Select("City").Distinct().From("Users").OrderBy("City").ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctAndJoin_ReturnsSqlWithDistinct()
    {
        SqlTable users = new("Users", "u");
        SqlTable profiles = new("Profiles", "p");

        const string expected =
            "SELECT DISTINCT [u].[City] FROM [Users] [u] INNER JOIN [Profiles] [p] ON [u].[Id] = [p].[UserId]";
        var actual = Sql.Select(users + "City")
            .Distinct()
            .From(users)
            .InnerJoin(profiles)
            .On(SqlExpression.Equal(users + "Id", profiles + "UserId"))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithoutDistinct_DoesNotOutputDistinct()
    {
        const string expected = "SELECT [City] FROM [Users]";
        var actual = Sql.Select("City").From("Users").ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctAndLimit_ReturnsSqlWithDistinct()
    {
        const string expected =
            "SELECT DISTINCT [City] FROM [Users] ORDER BY [City] OFFSET 10 ROWS FETCH FIRST 5 ROWS ONLY";
        var actual = Sql.Select("City")
            .Distinct()
            .From("Users")
            .OrderBy("City")
            .Limit(10, 5)
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithDistinctStar_ReturnsSqlWithDistinct()
    {
        const string expected = "SELECT DISTINCT * FROM [Users]";
        var actual = Sql.Select("*").Distinct().From("Users").ToSql();

        Assert.Equal(expected, actual);
    }
}
