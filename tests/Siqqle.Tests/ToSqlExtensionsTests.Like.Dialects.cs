using Siqqle.Dialects.MySql;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithNotLike_SqlServerDialect()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Name] NOT LIKE '%Admin%'";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotLike(new SqlColumn("Name"), new SqlConstant("%Admin%")))
            .ToSql(new SqlServerDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotLike_PostgreSqlDialect()
    {
        const string expected =
            "SELECT \"Id\", \"Name\" FROM \"Users\" WHERE \"Name\" NOT LIKE '%Admin%'";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotLike(new SqlColumn("Name"), new SqlConstant("%Admin%")))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotLike_MySqlDialect()
    {
        const string expected = "SELECT `Id`, `Name` FROM `Users` WHERE `Name` NOT LIKE '%Admin%'";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotLike(new SqlColumn("Name"), new SqlConstant("%Admin%")))
            .ToSql(new MySqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLike_PostgreSqlDialect()
    {
        const string expected =
            "SELECT \"Email\" FROM \"Users\" WHERE \"Email\" LIKE '%@example.com'";

        var actual = Sql.Select("Email")
            .From("Users")
            .Where(SqlExpression.Like(new SqlColumn("Email"), new SqlConstant("%@example.com")))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithLike_MySqlDialect()
    {
        const string expected = "SELECT `Email` FROM `Users` WHERE `Email` LIKE '%@example.com'";

        var actual = Sql.Select("Email")
            .From("Users")
            .Where(SqlExpression.Like(new SqlColumn("Email"), new SqlConstant("%@example.com")))
            .ToSql(new MySqlDialect());

        Assert.Equal(expected, actual);
    }
}
