using System;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlCallTests
{
    [Fact]
    public void Ctor_WithNullProcedureName_ThrowsArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SqlCall(null, null));
    }

    [Fact]
    public void Ctor_WithNonNullProcedureName_UpdatesProcedureNameProperty()
    {
        var expected = "sp_GetData";
        var call = new SqlCall("sp_GetData", null);

        Assert.Equal(expected, call.ProcedureName);
    }

    [Fact]
    public void Ctor_WithArguments_UpdatesArgumentsProperty()
    {
        var expected = new[] { new SqlConstant("param1"), new SqlConstant("param2") };
        var call = new SqlCall("sp_GetData", expected);

        Assert.NotNull(call.Arguments);
        Assert.Same(expected, call.Arguments);
    }

    [Fact]
    public void ExpressionType_ReturnsCall()
    {
        var call = new SqlCall("sp_GetData", null);

        Assert.Equal(SqlExpressionType.Call, call.ExpressionType);
    }
}
