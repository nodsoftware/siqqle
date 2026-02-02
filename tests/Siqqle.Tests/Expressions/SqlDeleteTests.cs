using System;
using System.Linq;
using NSubstitute;
using Siqqle.Expressions.Builders;
using Siqqle.Expressions.Visitors;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlDeleteTests
{
    [Fact]
    public void Ctor_DoesNotThrow()
    {
        new SqlDelete();
    }

    [Fact]
    public void From_WithNullTable_ThrowsArgumentNull()
    {
        var delete = Sql.Delete();
        Assert.Throws<ArgumentNullException>(() => delete.From(null));
    }

    [Fact]
    public void From_WithTable_SetsFromProperty()
    {
        var delete = Sql.Delete().From("Users").Go();

        Assert.NotNull(delete.From);
        Assert.Equal("Users", ((SqlTable)delete.From.Table).TableName.Segments.First());
    }

    [Fact]
    public void Where_WithExpression_SetsWhereProperty()
    {
        var delete = Sql.Delete().From("User").Where(SqlExpression.Equal("Id", 5)).Go();

        Assert.NotNull(delete.Where);
        Assert.NotNull(delete.Where.Predicate);
        Assert.IsType<SqlBinaryExpression>(delete.Where.Predicate);
        Assert.IsType<SqlColumn>(((SqlBinaryExpression)delete.Where.Predicate).Left);
        Assert.Equal(
            "Id",
            (
                (SqlColumn)((SqlBinaryExpression)delete.Where.Predicate).Left
            ).ColumnName.Segments.First()
        );

        Assert.Equal(
            SqlBinaryOperator.Equal,
            ((SqlBinaryExpression)delete.Where.Predicate).Operator
        );

        Assert.IsType<SqlConstant>(((SqlBinaryExpression)delete.Where.Predicate).Right);
        Assert.Equal(5, ((SqlConstant)((SqlBinaryExpression)delete.Where.Predicate).Right).Value);
    }

    [Fact]
    public void ExpressionType_ReturnsDelete()
    {
        var query = new SqlDelete();

        Assert.Equal(SqlExpressionType.Delete, query.ExpressionType);
    }

    [Fact]
    public void Accept_WithoutFrom_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();
        var query = new SqlDelete();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlDelete>());
        mock.DidNotReceive().Visit(Arg.Any<SqlFrom>());
        mock.DidNotReceive().Visit(Arg.Any<SqlWhere>());
    }

    [Fact]
    public void Accept_WithoutWhere_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();
        var query = Sql.Delete().From("User").Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlDelete>());
        mock.Received(1).Visit(Arg.Any<SqlFrom>());
        mock.Received(1).Visit(Arg.Any<SqlTable>());
        mock.DidNotReceive().Visit(Arg.Any<SqlWhere>());
    }

    [Fact]
    public void Accept_WithWhere_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();

        var query = Sql.Delete().From("User").Where(SqlExpression.Equal("Id", 5)).Go();

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlDelete>());
        mock.Received(1).Visit(Arg.Any<SqlFrom>());
        mock.Received(1).Visit(Arg.Any<SqlTable>());
        mock.Received(1).Visit(Arg.Any<SqlWhere>());
    }
}
