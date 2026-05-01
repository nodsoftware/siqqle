using System;
using System.Collections;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlSetTests
{
    [Fact]
    public void Add_WithNullAssign_ThrowsArgumentNull()
    {
        var set = new SqlSet();
        Assert.Throws<ArgumentNullException>(() => set.Add(null));
    }

    [Fact]
    public void Count_ReturnsNumberOfItems()
    {
        var values = new SqlSet();
        Assert.Empty(values);

        values.Add(new SqlAssign("Name", "John Doe"));
        Assert.Single(values);

        values.Add(new SqlAssign("Email", "john@d.oe"));
        Assert.Equal(2, values.Count);
    }

    [Fact]
    public void GetEnumerator_ReturnsEnumerator()
    {
        var values = new SqlSet
        {
            new SqlAssign("Name", "John Doe"),
            new SqlAssign("Email", "john@d.oe"),
        };

        var enumerator = ((IEnumerable)values).GetEnumerator();
        Assert.NotNull(enumerator);

        int count = 0;
        while (enumerator.MoveNext())
        {
            count++;
        }
        Assert.Equal(2, count);
    }

    [Fact]
    public void ExpressionType_ReturnsSet()
    {
        Assert.Equal(SqlExpressionType.Set, new SqlSet().ExpressionType);
    }
}
