using System.Text;
using Siqqle.Dialects.MySql;
using Siqqle.Expressions;
using Siqqle.Text;
using Xunit;

namespace Siqqle.Dialects.Tests;

public class MySqlDialectTests
{
    [Fact]
    public void FormatIdentifier_EnclosesInBackticks()
    {
        var dialect = new MySqlDialect();
        var result = dialect.FormatIdentifier("TableName");

        Assert.Equal("`TableName`", result);
    }

    [Fact]
    public void FormatParameterName_PrefixesWithQuestionMark()
    {
        var dialect = new MySqlDialect();
        var result = dialect.FormatParameterName("paramName");

        Assert.Equal("?paramName", result);
    }

    [Theory]
    [InlineData("DATE", "DATE")]
    [InlineData("TIME", "TIME")]
    [InlineData("DATETIME", "DATETIME")]
    [InlineData("DATETIME2", "DATETIME")]
    [InlineData("DATETIMEOFFSET", "DATETIME")]
    [InlineData("SMALLDATETIME", "DATETIME")]
    [InlineData("INT", "INT")]
    [InlineData("VARCHAR", "VARCHAR")]
    public void GetDataTypeName_ReturnsMappedTypeName(string input, string expected)
    {
        var dialect = new MySqlDialect();
        var result = dialect.GetDataTypeName(new SqlDataTypeName(input));
        Assert.Equal(expected, result.Name);
    }

    [Fact]
    public void ToSql_CastWithDate_EmitsDate()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"BirthDate", SqlDataType.Date()))
            .From("Persons")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`BirthDate` AS DATE) FROM `Persons`", sql);
    }

    [Fact]
    public void ToSql_CastWithTime_EmitsTime()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"StartTime", SqlDataType.Time(3)))
            .From("Events")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`StartTime` AS TIME(3)) FROM `Events`", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTime_EmitsDateTime()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTime()))
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`CreatedAt` AS DATETIME) FROM `Orders`", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTime2_EmitsDateTime()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTime2(3)))
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`CreatedAt` AS DATETIME(3)) FROM `Orders`", sql);
    }

    [Fact]
    public void ToSql_CastWithDateTimeOffset_EmitsDateTime()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(
                SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.DateTimeOffset())
            )
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`CreatedAt` AS DATETIME) FROM `Orders`", sql);
    }

    [Fact]
    public void ToSql_CastWithSmallDateTime_EmitsDateTime()
    {
        var dialect = new MySqlDialect();
        var sql = Sql.Select(
                SqlExpression.Cast((SqlColumn)"CreatedAt", SqlDataType.SmallDateTime())
            )
            .From("Orders")
            .ToSql(dialect);
        Assert.Equal("SELECT CAST(`CreatedAt` AS DATETIME) FROM `Orders`", sql);
    }
}
