using System;
using System.Linq;
using NSubstitute;
using Siqqle.Expressions.Builders;
using Siqqle.Expressions.Visitors;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlInsertTests
{
    [Fact]
    public void Ctor_DoesNotThrow()
    {
        _ = new SqlInsert();
    }

    [Fact]
    public void Into_WithNullTable_ThrowsArgumentNull()
    {
        var insert = Sql.Insert();
        Assert.Throws<ArgumentNullException>(() => insert.Into(null));
    }

    [Fact]
    public void Into_WithTable_SetsIntoProperty()
    {
        var insert = Sql.Insert().Into("Users").Values(1, "John Doe").Go();

        Assert.NotNull(insert.Into);
        Assert.Equal("Users", ((SqlTable)insert.Into.Table).TableName.Segments.First());
    }

    [Fact]
    public void Into_WithNullTableAndColumns_ThrowsArgumentNull()
    {
        var insert = Sql.Insert();

        Assert.Throws<ArgumentNullException>(() => insert.Into(null, "Id", "Name"));
    }

    [Fact]
    public void Into_WithTableAndColumns_SetsColumnsProperty()
    {
        var insert = Sql.Insert().Into("Users", "Id", "Name").Values(1, "John Doe").Go();

        Assert.NotNull(insert.Columns);
        Assert.Equal(2, insert.Columns.Count());
        Assert.Equal("Id", insert.Columns.First().ColumnName.Segments.First());
        Assert.Equal("Name", insert.Columns.Last().ColumnName.Segments.First());
    }

    [Fact]
    public void Values_WithNullValues_ThrowsArgumentNull()
    {
        var insert = Sql.Insert().Into("User");

        Assert.Throws<ArgumentNullException>(() => insert.Values(null));
    }

    [Fact]
    public void Values_SetsValuesProperty()
    {
        var insert = Sql.Insert().Into("User").Values(1, "John Doe").Go();

        Assert.NotNull(insert.Values);
        Assert.Single(insert.Values);
        Assert.Equal(2, insert.Values.First().Count());
        Assert.Equal(1, insert.Values.First().First());
        Assert.Equal("John Doe", insert.Values.First().Last());
    }

    [Fact]
    public void Values_SetsValuesPropertyRepeatedly()
    {
        var insert = Sql.Insert()
            .Into("User")
            .Values(1, "John Doe")
            .Values(2, "Jane Doe")
            .Values(3, "Junior Doe")
            .Go();

        Assert.NotNull(insert.Values);
        Assert.Equal(3, insert.Values.Count);
        Assert.Equal(2, insert.Values.First().Count());
        Assert.Equal(2, insert.Values.Skip(1).First().Count());
        Assert.Equal(2, insert.Values.Last().Count());

        Assert.Equal(1, insert.Values.First().First());
        Assert.Equal("John Doe", insert.Values.First().Last());

        Assert.Equal(2, insert.Values.Skip(1).First().First());
        Assert.Equal("Jane Doe", insert.Values.Skip(1).First().Last());

        Assert.Equal(3, insert.Values.Last().First());
        Assert.Equal("Junior Doe", insert.Values.Last().Last());
    }

    [Fact]
    public void Columns_IsNullWhenFirstCalled()
    {
        var insert = new SqlInsert();
        Assert.Null(insert.Columns);
    }

    [Fact]
    public void Values_IsNotNullWhenFirstCalled()
    {
        var insert = new SqlInsert();
        Assert.NotNull(insert.Values);
    }

    [Fact]
    public void ExpressionType_ReturnsSelect()
    {
        var query = new SqlInsert();

        Assert.Equal(SqlExpressionType.Insert, query.ExpressionType);
    }

    [Fact]
    public void Accept_WithoutInto_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();

        var query = new SqlInsert();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlInsert>());
        mock.DidNotReceive().Visit(Arg.Any<SqlInto>());
        mock.DidNotReceive().Visit(Arg.Any<SqlColumn>());
        mock.Received(1).Visit(Arg.Any<SqlValues>());
    }

    [Fact]
    public void Accept_WithColumns_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();
        var query = Sql.Insert().Into("User").Values(1).Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlInsert>());
        mock.Received(1).Visit(Arg.Any<SqlInto>());
        mock.DidNotReceive().Visit(Arg.Any<SqlColumn>());
        mock.Received(1).Visit(Arg.Any<SqlValues>());
    }
}
