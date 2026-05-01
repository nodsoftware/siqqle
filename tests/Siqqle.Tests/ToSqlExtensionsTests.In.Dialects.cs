using Siqqle.Dialects.MySql;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Dialects.SqlServer;
using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithNotIn_SqlServerDialect()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE [Status] NOT IN ('Active', 'Pending')";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotIn("Status", "Active", "Pending"))
            .ToSql(new SqlServerDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotIn_PostgreSqlDialect()
    {
        const string expected =
            "SELECT \"Id\", \"Name\" FROM \"Users\" WHERE \"Status\" NOT IN ('Active', 'Pending')";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotIn("Status", "Active", "Pending"))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotIn_MySqlDialect()
    {
        const string expected =
            "SELECT `Id`, `Name` FROM `Users` WHERE `Status` NOT IN ('Active', 'Pending')";

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotIn("Status", "Active", "Pending"))
            .ToSql(new MySqlDialect());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotInSubquery_PostgreSqlDialect()
    {
        const string expected =
            "SELECT \"Id\", \"Name\" FROM \"Users\" WHERE \"DepartmentId\" NOT IN (SELECT \"Id\" FROM \"Departments\" WHERE \"IsActive\" = 1)";

        var subquery = new SqlSubquery(
            Sql.Select("Id").From("Departments").Where(SqlExpression.Equal("IsActive", 1))
        );

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotIn("DepartmentId", subquery))
            .ToSql(new PostgreSqlDialect());

        Assert.Equal(expected, actual);
    }
}
