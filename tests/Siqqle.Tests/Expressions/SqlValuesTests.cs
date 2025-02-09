using System;
using System.Collections;
using System.Linq;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlValuesTests
{
    [Fact]
    public void Add_WithNullValues_ThrowsArgumentNull()
    {
        var values = new SqlValues();
        Assert.Throws<ArgumentNullException>(() => values.Add(null));
    }

    [Fact]
    public void Count_ReturnsNumberOfItems()
    {
        var values = new SqlValues();
        Assert.Empty(values);

        values.Add(Enumerable.Range(0, 5).Select(i => (SqlValue)i));
        Assert.Single(values);

        values.Add(Enumerable.Range(5, 10).Select(i => (SqlValue)i));
        Assert.Equal(2, values.Count);
    }

    [Fact]
    public void ExpressionType_ReturnsValues()
    {
        Assert.Equal(SqlExpressionType.Values, new SqlValues().ExpressionType);
    }

    [Fact]
    public void GetEnumerator_ReturnsEnumerator()
    {
        var values = new SqlValues
        {
            Enumerable.Range(0, 5).Select(i => (SqlValue)i),
            Enumerable.Range(5, 10).Select(i => (SqlValue)i),
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
}
