using System;
using System.Collections;
using NSubstitute;
using Siqqle.Expressions.Builders;
using Siqqle.Expressions.Visitors;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlGroupByTests
{
    [Fact]
    public void Ctor_WithNullColumn_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlGroupBy(null));
    }

    [Fact]
    public void ExpressionType_ReturnsOrderBy()
    {
        var groupBy = new SqlGroupBy("Name");
        Assert.Equal(SqlExpressionType.GroupBy, groupBy.ExpressionType);
    }

    [Fact]
    public void GetEnumerator_ReturnsEnumerator()
    {
        var values = new SqlGroupBy("Age");

        var enumerator = ((IEnumerable)values).GetEnumerator();
        Assert.NotNull(enumerator);

        int count = 0;
        while (enumerator.MoveNext())
        {
            count++;
        }
        Assert.Equal(1, count);
    }

    [Fact]
    public void Accept_WithHaving_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();

        var query = Sql.Select("Age")
            .From("User")
            .GroupBy("Age")
            .Having(SqlExpression.GreaterThanOrEqual("Age", 18))
            .OrderBy("Age", SqlSortOrder.Descending)
            .Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlSelect>());
        mock.Received(1).Visit(Arg.Any<SqlFrom>());
        mock.Received(1).Visit(Arg.Any<SqlGroupBy>());
        mock.Received(1).Visit(Arg.Any<SqlHaving>());
        mock.Received(1).Visit(Arg.Any<SqlOrderBy>());
    }

    [Fact]
    public void Accept_WithoutHaving_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();

        var query = Sql.Select("Age")
            .From("User")
            .GroupBy("Age")
            .OrderBy("Age", SqlSortOrder.Descending)
            .Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlSelect>());
        mock.Received(1).Visit(Arg.Any<SqlFrom>());
        mock.Received(1).Visit(Arg.Any<SqlGroupBy>());
        mock.DidNotReceive().Visit(Arg.Any<SqlHaving>());
        mock.Received(1).Visit(Arg.Any<SqlOrderBy>());
    }

    [Fact]
    public void Accept_WithCast_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();

        var query = Sql.Select(SqlExpression.Cast((SqlColumn)"Age", SqlDataType.BigInt()))
            .From("User")
            .Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlCast>());
    }
}
