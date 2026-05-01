using Siqqle.Dialects.SqlServer;
using Siqqle.Expressions;
using Xunit;

namespace Siqqle.Dialects.Tests;

public class SqlServerDialectTests
{
    [Theory]
    [InlineData("DATE", "DATE")]
    [InlineData("TIME", "TIME")]
    [InlineData("DATETIME", "DATETIME")]
    [InlineData("DATETIME2", "DATETIME2")]
    [InlineData("DATETIMEOFFSET", "DATETIMEOFFSET")]
    [InlineData("SMALLDATETIME", "SMALLDATETIME")]
    [InlineData("INT", "INT")]
    [InlineData("VARCHAR", "VARCHAR")]
    public void GetDataTypeName_ReturnsNameUnchanged(string input, string expected)
    {
        var dialect = new SqlServerDialect();
        var result = dialect.GetDataTypeName(new SqlDataTypeName(input));
        Assert.Equal(expected, result.Name);
    }

    [Fact]
    public void ToSql_CastWithDate_EmitsDate()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"BirthDate", SqlDataType.Date()))
            .From("Persons")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([BirthDate] AS DATE) FROM [Persons]", sql);
    }

    [Fact]
    public void ToSql_CastWithTime_EmitsTime()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"StartTime", SqlDataType.Time(3)))
            .From("Events")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([StartTime] AS TIME(3)) FROM [Events]", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTime_EmitsDateTime()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTime()))
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([CreatedAt] AS DATETIME) FROM [Orders]", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTime2WithPrecision_EmitsDateTime2WithPrecision()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTime2(7)))
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([CreatedAt] AS DATETIME2(7)) FROM [Orders]", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTimeOffset_EmitsDateTimeOffset()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(
                SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTimeOffset(7))
            )
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([CreatedAt] AS DATETIMEOFFSET(7)) FROM [Orders]", sql);
    }

    [Fact]
    public void ToSql_CastWithSmallDateTime_EmitsSmallDateTime()
    {
        var dialect = new SqlServerDialect();
        var sql = Sql.Select(
                SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.SmallDateTime())
            )
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST([CreatedAt] AS SMALLDATETIME) FROM [Orders]", sql);
    }
}
