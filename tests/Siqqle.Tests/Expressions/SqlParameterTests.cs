using System;
using System.Data;
using Xunit;

namespace Siqqle.Expressions.Tests;

public class SqlParameterTests
{
    // Test enum to simulate provider-specific types
    private enum TestDbType
    {
        Jsonb,
        Uuid,
        Xml,
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Ctor_WithNullOrWhiteSpaceParameterName_ThrowsArgumentNull(string parameterName)
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new SqlParameter(parameterName, null));
        Assert.Equal(nameof(parameterName), ex.ParamName);
    }

    [Fact]
    public void Ctor_WithParameterNameAndValue_SetsPropertyValues()
    {
        var value = Guid.NewGuid();
        var parameter = new SqlParameter("Id", value);

        Assert.NotNull(parameter.ParameterName);
        Assert.Equal("Id", parameter.ParameterName);
        Assert.NotNull(parameter.Value);
        Assert.Equal(value, parameter.Value);
        Assert.Null(parameter.DbType);
    }

    [Fact]
    public void Ctor_WithDbType_SetsPropertyValues()
    {
        var parameter = new SqlParameter("data", DbType.Xml, "<root/>");

        Assert.Equal("data", parameter.ParameterName);
        Assert.Equal("<root/>", parameter.Value);
        Assert.Equal(DbType.Xml, parameter.DbType);
    }

    [Fact]
    public void GenericCtor_WithProviderSpecificType_SetsPropertyValues()
    {
        var parameter = new SqlParameter<TestDbType>("data", TestDbType.Jsonb, "{}");

        Assert.Equal("data", parameter.ParameterName);
        Assert.Equal("{}", parameter.Value);
        Assert.Equal(TestDbType.Jsonb, parameter.ProviderDbType);
    }

    [Fact]
    public void GenericCtor_WithProviderSpecificType_InheritsFromSqlParameter()
    {
        var parameter = new SqlParameter<TestDbType>("id", TestDbType.Uuid, Guid.NewGuid());

        Assert.IsAssignableFrom<SqlParameter>(parameter);
        Assert.IsType<SqlParameter<TestDbType>>(parameter);
    }

    [Fact]
    public void GenericParameter_ProviderDbType_IsStronglyTyped()
    {
        var parameter = new SqlParameter<TestDbType>("data", TestDbType.Jsonb, "{}");

        // ProviderDbType is strongly typed
        TestDbType dbType = parameter.ProviderDbType;
        Assert.Equal(TestDbType.Jsonb, dbType);
    }

    [Fact]
    public void ExpressionType_ReturnsParameter()
    {
        var parameter = new SqlParameter("Id", 5);
        Assert.Equal(SqlExpressionType.Parameter, parameter.ExpressionType);
    }

    [Fact]
    public void ExpressionType_OnGenericParameter_ReturnsParameter()
    {
        var parameter = new SqlParameter<TestDbType>("Id", TestDbType.Uuid, Guid.NewGuid());
        Assert.Equal(SqlExpressionType.Parameter, parameter.ExpressionType);
    }

    [Fact]
    public void GenericParameter_CanBeUsedWithDifferentEnums()
    {
        // Simulate different provider enums
        var pgParam = new SqlParameter<TestDbType>("data1", TestDbType.Jsonb, "{}");
        var xmlParam = new SqlParameter<TestDbType>("data2", TestDbType.Xml, "<xml/>");

        Assert.Equal(TestDbType.Jsonb, pgParam.ProviderDbType);
        Assert.Equal(TestDbType.Xml, xmlParam.ProviderDbType);
    }

    [Fact]
    public void GenericParameter_BaseProperties_AreAccessible()
    {
        var parameter = new SqlParameter<TestDbType>("test", TestDbType.Jsonb, "value");

        // Should have access to base class properties
        Assert.Equal("test", parameter.ParameterName);
        Assert.Equal("value", parameter.Value);
        Assert.Null(parameter.DbType);
    }
}
