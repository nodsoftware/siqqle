using System;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlSortTests
{
    [Fact]
    public void Ctor_WithNullColumn_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlSort(null, SqlSortOrder.Ascending));
    }

    [Fact]
    public void Ctor_WithColumn_SetsPropertyValues()
    {
        SqlColumn column = "dbo.Users.CreatedOn";
        SqlSort sort = new(column, SqlSortOrder.Descending);

        Assert.NotNull(sort.Column);
        Assert.Same(column, sort.Column);
        Assert.Equal(SqlSortOrder.Descending, sort.SortOrder);
    }

    [Fact]
    public void ExpressionType_ReturnsSort()
    {
        var sort = new SqlSort("dbo.Users.Id", SqlSortOrder.Ascending);
        Assert.Equal(SqlExpressionType.Sort, sort.ExpressionType);
    }
}
