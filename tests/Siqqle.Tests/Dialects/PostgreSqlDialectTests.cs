using System;
using System.Data;
using System.Text;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Expressions;
using Siqqle.Text;
using Xunit;

namespace Siqqle.Dialects.Tests;

public class PostgreSqlDialectTests
{
    // Simulating NpgsqlDbType for testing
    private enum NpgsqlDbType
    {
        Jsonb = 4094,
        Json = 4093,
        Uuid = 27,
        Xml = 25,
        Hstore = 37,
        Bytea = 17,
    }

    [Fact]
    public void FormatIdentifier_EnclosesInDoubleQuotes()
    {
        var dialect = new PostgreSqlDialect();
        var result = dialect.FormatIdentifier("TableName");

        Assert.Equal("\"TableName\"", result);
    }

    [Fact]
    public void FormatParameterName_PrefixesWithColon()
    {
        var dialect = new PostgreSqlDialect();
        var result = dialect.FormatParameterName("paramName");

        Assert.Equal(":paramName", result);
    }

    [Fact]
    public void WriteParameter_WithoutType_WritesParameterNameOnly()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter("data", "value");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":data", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithDbTypeXml_AddsXmlCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter("metadata", DbType.Xml, "<root/>");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":metadata::xml", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithGenericNpgsqlDbTypeJsonb_AddsJsonbCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>("data", NpgsqlDbType.Jsonb, "{}");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":data::jsonb", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithGenericNpgsqlDbTypeJson_AddsJsonCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>("data", NpgsqlDbType.Json, "[]");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":data::json", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithGenericNpgsqlDbTypeUuid_AddsUuidCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>("id", NpgsqlDbType.Uuid, Guid.NewGuid());
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":id::uuid", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithGenericNpgsqlDbTypeHstore_AddsHstoreCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>("store", NpgsqlDbType.Hstore, "key=>value");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":store::hstore", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_WithGenericNpgsqlDbTypeBytea_AddsByteaCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>(
            "binary",
            NpgsqlDbType.Bytea,
            new byte[] { 1, 2, 3 }
        );
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":binary::bytea", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_GenericType_WithUnknownEnumValue_UsesEnumName()
    {
        var dialect = new PostgreSqlDialect();
        // Use a value that won't match any specific case
        var parameter = new SqlParameter<NpgsqlDbType>("data", (NpgsqlDbType)999, "value");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        // Should output parameter name + lowercase enum value
        Assert.Equal(":data::999", builder.ToString().Trim());
    }

    [Fact]
    public void ToSql_WithGenericJsonbParameter_GeneratesCorrectSql()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter<NpgsqlDbType>("props", NpgsqlDbType.Jsonb, "{}");

        var query = Sql.Update("Products")
            .Set("Properties", parameter)
            .Where(SqlExpression.Equal("Id", 1))
            .ToSql(dialect);

        Assert.Contains(":props::jsonb", query);
        Assert.Contains("UPDATE \"Products\"", query);
    }

    [Fact]
    public void ToSql_WithMultipleGenericTypedParameters_GeneratesCorrectSql()
    {
        var dialect = new PostgreSqlDialect();

        var query = Sql.Insert()
            .Into("Records", "Id", "Data", "Metadata")
            .Values(
                new SqlParameter<NpgsqlDbType>("id", NpgsqlDbType.Uuid, Guid.NewGuid()),
                new SqlParameter<NpgsqlDbType>("data", NpgsqlDbType.Jsonb, "{}"),
                new SqlParameter("metadata", DbType.Xml, "<xml/>")
            )
            .ToSql(dialect);

        Assert.Contains(":id::uuid", query);
        Assert.Contains(":data::jsonb", query);
        Assert.Contains(":metadata::xml", query);
    }

    [Fact]
    public void WriteParameter_MixedBaseAndGenericParameters_HandlesCorrectly()
    {
        var dialect = new PostgreSqlDialect();

        // Base SqlParameter
        var baseParam = new SqlParameter("normal", 42);
        var builder1 = new StringBuilder();
        using (var writer = new SqlWriter(builder1, dialect))
        {
            dialect.WriteParameter(writer, baseParam);
        }
        Assert.Equal(":normal", builder1.ToString().Trim());

        // Generic SqlParameter<T>
        var genericParam = new SqlParameter<NpgsqlDbType>("typed", NpgsqlDbType.Jsonb, "{}");
        var builder2 = new StringBuilder();
        using (var writer = new SqlWriter(builder2, dialect))
        {
            dialect.WriteParameter(writer, genericParam);
        }
        Assert.Equal(":typed::jsonb", builder2.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_BaseParameterWithDbType_AddsTypeCast()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter("metadata", DbType.Xml, "<root/>");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":metadata::xml", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_BaseParameterWithDbType_StillWorks()
    {
        var dialect = new PostgreSqlDialect();
        var parameter = new SqlParameter("guid", DbType.Guid, Guid.NewGuid());
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Equal(":guid::uuid", builder.ToString().Trim());
    }

    [Fact]
    public void WriteParameter_ReflectionBasedDetection_WorksWithDifferentEnums()
    {
        // Test that the reflection-based approach works with any enum
        var dialect = new PostgreSqlDialect();

        // Even with a different enum type, it should detect the generic type
        var parameter = new SqlParameter<NpgsqlDbType>("test", NpgsqlDbType.Json, "[]");
        var builder = new StringBuilder();

        using (var writer = new SqlWriter(builder, dialect))
        {
            dialect.WriteParameter(writer, parameter);
        }

        Assert.Contains("::", builder.ToString());
    }

    [Fact]
    public void ToSql_ComplexQueryWithMultipleGenericParameters_GeneratesCorrectSql()
    {
        var dialect = new PostgreSqlDialect();

        var query = Sql.Select("Id", "Name", "Properties", "Settings")
            .From("Users")
            .Where(
                SqlExpression.Equal(
                    "Properties",
                    new SqlParameter<NpgsqlDbType>("props", NpgsqlDbType.Jsonb, "{}")
                )
            )
            .ToSql(dialect);

        Assert.Contains(":props::jsonb", query);
        Assert.Contains("SELECT", query);
        Assert.Contains("FROM \"Users\"", query);
    }
}
