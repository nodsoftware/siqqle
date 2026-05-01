using Siqqle.Dialects.MySql;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithIsNull_SqlServerDialect()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Email] IS NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNull(new SqlColumn("Email")))
            .ToSql(new SqlServerDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNull_PostgreSqlDialect()
    {
        const string expected = "SELECT \"Id\", \"Name\" FROM \"Users\" WHERE \"Email\" IS NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNull(new SqlColumn("Email")))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNull_MySqlDialect()
    {
        const string expected = "SELECT `Id`, `Name` FROM `Users` WHERE `Email` IS NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNull(new SqlColumn("Email")))
            .ToSql(new MySqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNull_SqlServerDialect()
    {
        const string expected = "SELECT [Id], [Name] FROM [Users] WHERE [Email] IS NOT NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNotNull(new SqlColumn("Email")))
            .ToSql(new SqlServerDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNull_PostgreSqlDialect()
    {
        const string expected =
            "SELECT \"Id\", \"Name\" FROM \"Users\" WHERE \"Email\" IS NOT NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNotNull(new SqlColumn("Email")))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithIsNotNull_MySqlDialect()
    {
        const string expected = "SELECT `Id`, `Name` FROM `Users` WHERE `Email` IS NOT NULL";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.IsNotNull(new SqlColumn("Email")))
            .ToSql(new MySqlDialect());

        Assert.Equal(expected, actual);
    }
}
