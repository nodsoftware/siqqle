# Parameters

Named parameters protect your application against SQL injection and allow values to be bound by the database driver rather than embedded in the SQL text.

## Creating a parameter

A `SqlParameter` is created using the `+` operator between a parameter name string and a `SqlConstant`:

```csharp
// "ParameterName" + (SqlConstant)value  →  SqlParameter
SqlParameter p = "UserId" + (SqlConstant)42;
```

You can use a parameter anywhere a value is expected:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Id] = @UserId
```

The value (`42`) travels with the parameter and is accessible through the `SqlParameter.Value` property when the expression tree is visited.

## Reading parameter values with a callback

Pass a callback to `ToSql()` to receive each parameter as it is visited. This is the low-level way to integrate with ADO.NET:

```csharp
var command = connection.CreateCommand();

var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
    .ToSql(p =>
    {
        var dbParam = command.CreateParameter();
        dbParam.ParameterName = p.ParameterName;  // "UserId"
        dbParam.Value = p.Value;                  // 42
        command.Parameters.Add(dbParam);
    });

command.CommandText = sql;
```

### Complete ADO.NET example

```csharp
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using var command = connection.CreateCommand();

command.CommandText = Sql
    .Select("Id", "Name", "Email")
    .From("Users")
    .Where(
        SqlExpression.And(
            SqlExpression.Equal("IsActive", "IsActive" + (SqlConstant)true),
            SqlExpression.GreaterThanOrEqual("Age", "MinAge" + (SqlConstant)18)
        )
    )
    .OrderBy("Name")
    .ToSql(p =>
    {
        var dbParam = command.CreateParameter();
        dbParam.ParameterName = p.ParameterName;
        dbParam.Value = p.Value;
        if (p.DbType.HasValue)
            dbParam.DbType = p.DbType.Value;
        command.Parameters.Add(dbParam);
    });

using var reader = await command.ExecuteReaderAsync();
while (await reader.ReadAsync())
{
    Console.WriteLine($"{reader["Id"]}: {reader["Name"]}");
}
```

## Specifying a DbType

Construct a `SqlParameter` directly when you need to specify the database type explicitly:

```csharp
var param = new SqlParameter("StartDate", DbType.Date, new DateOnly(2024, 1, 1));

var sql = Sql
    .Select("Id", "Name")
    .From("Orders")
    .Where(SqlExpression.GreaterThanOrEqual("OrderDate", param))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Orders] WHERE [OrderDate] >= @StartDate
```

The `DbType` is available on the `SqlParameter.DbType` property in the callback, so you can pass it through to the underlying `DbParameter`.

## SqlParameter constructors

| Constructor | Description |
|---|---|
| `new SqlParameter(name, value)` | Name and value only |
| `new SqlParameter(name, dbType, value)` | Name, explicit `DbType`, and value |
| `"Name" + (SqlConstant)value` | Shorthand via operator overload |

## PostgreSQL typed parameters

PostgreSQL sometimes requires a type cast on a parameter (e.g. `::jsonb`). Use the generic `SqlParameter<T>` class with an `NpgsqlDbType` value:

```csharp
var param = new SqlParameter<NpgsqlDbType>("Payload", NpgsqlDbType.Jsonb, "{}");

var sql = Sql
    .Update("Events")
    .Set("Payload", param)
    .Where(SqlExpression.Equal("Id", "EventId" + (SqlConstant)1))
    .ToSql(new PostgreSqlDialect());
```

```sql
UPDATE "Events" SET "Payload" = :Payload::jsonb WHERE "Id" = :EventId
```

The `PostgreSqlDialect` automatically appends the type cast based on the `NpgsqlDbType` value. It also handles `DbType.Guid` → `::uuid` and `DbType.Xml` → `::xml` for standard `SqlParameter` instances.

## Multiple parameters

Each unique parameter name appears once in the rendered SQL. Use distinct names when a single query contains multiple parameters:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(
        SqlExpression.And(
            SqlExpression.GreaterThanOrEqual("Age", "MinAge" + (SqlConstant)18),
            SqlExpression.LessThanOrEqual("Age", "MaxAge" + (SqlConstant)65)
        )
    )
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE ([Age] >= @MinAge) AND ([Age] <= @MaxAge)
```

## Parameters in INSERT

```csharp
var sql = Sql
    .Insert()
    .Into("Users", "UserName", "Email")
    .Values(
        "UserName" + (SqlConstant)"jdoe",
        "Email" + (SqlConstant)"j@example.com"
    )
    .ToSql();
```

```sql
INSERT INTO [Users] ([UserName], [Email]) VALUES (@UserName, @Email)
```

## Parameters in UPDATE

```csharp
var sql = Sql
    .Update("Users")
    .Set("Email", "NewEmail" + (SqlConstant)"new@example.com")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
    .ToSql();
```

```sql
UPDATE [Users] SET [Email] = @NewEmail WHERE [Id] = @UserId
```

## Parameters in BETWEEN

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Products")
    .Where(
        SqlExpression.Between(
            "Price",
            "MinPrice" + (SqlConstant)10.0m,
            "MaxPrice" + (SqlConstant)100.0m
        )
    )
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Products] WHERE [Price] BETWEEN @MinPrice AND @MaxPrice
```

## Dialect-specific parameter prefixes

The parameter prefix changes depending on the dialect:

| Dialect | Prefix | Example |
|---|---|---|
| Default / SQL Server | `@` | `@UserId` |
| PostgreSQL | `:` | `:UserId` |
| MySQL | `?` | `?UserId` |

## Dapper integration

When using `Siqqle.Dapper`, parameter handling is automatic — Siqqle extracts parameters from the expression tree and feeds them to Dapper's `DynamicParameters`. No callback is needed.

`DateOnly` values are automatically converted to `DateTime` with `DbType.Date` for maximum driver compatibility.

See [Dapper Integration](dapper.md) for details.
