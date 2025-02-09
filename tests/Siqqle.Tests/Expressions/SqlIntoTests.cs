using System;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlIntoTests
{
    [Fact]
    public void Ctor_WithNullTable_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlInto(null));
    }
}
