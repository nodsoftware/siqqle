using System;
using System.Linq;
using NSubstitute;
using Siqqle.Expressions.Builders;
using Siqqle.Expressions.Visitors;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlUnionTests
{
    [Fact]
    public void Ctor_WithNullStatements_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlUnion(null));
    }

    [Fact]
    public void Ctor_WithEmptyStatements_ThrowsArgument()
    {
        Assert.Throws<ArgumentException>(() => new SqlUnion(Enumerable.Empty<SqlSelect>()));
    }

    [Fact]
    public void Ctor_WithStatementArray_SetsStatementsProperty()
    {
        var select1 = Sql.Select("Id").From("User").Go();
        var select2 = Sql.Select("Id").From("Post").Go();
        var select3 = Sql.Select("Id").From("Comment").Go();
        var union = new SqlUnion(select1, select2, select3);
        Assert.NotNull(union);
        Assert.Equal(3, union.Statements.Count());
        Assert.Same(select1, union.Statements.First());
        Assert.Same(select2, union.Statements.Skip(1).First());
        Assert.Same(select3, union.Statements.Last());
    }

    [Fact]
    public void ExpressionType_ReturnsUnion()
    {
        Assert.Equal(
            SqlExpressionType.Union,
            new SqlUnion(Sql.Select("Id").From("User").Go()).ExpressionType
        );
    }

    [Fact]
    public void Accept_WithWhere_VisitsEverything()
    {
        var mock = Substitute.ForPartsOf<SqlVisitor>();
        mock.When(x => x.Visit(Arg.Any<SqlExpression>())).CallBase();

        var query = Sql.Union(
            Sql.Select("Name").From("Supplier").Go(),
            Sql.Select("Name").From("Customer").Go()
        );

        query.Accept(mock);

        mock.Received(1).Visit(Arg.Any<SqlUnion>());
    }
}
