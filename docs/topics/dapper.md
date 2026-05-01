# Dapper Integration

`Siqqle.Dapper` adds extension methods to `IDbConnection` that accept Siqqle statements directly. Parameter extraction from the expression tree is handled automatically — you do not need to write a `parameterCallback`.

## Installation

```
dotnet add package Siqqle.Dapper
```

Add the namespace:

```csharp
using Siqqle.Dapper;
```

## Querying

### Return dynamic rows

```csharp
using var connection = OpenConnection();

var results = connection.Query(
    Sql.Select("Id", "Name", "Email")
       .From("Users")
       .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
       .OrderBy("Name")
);

foreach (dynamic row in results)
{
    Console.WriteLine($"{row.Id}: {row.Name}");
}
```

### Return typed objects

```csharp
var users = connection.Query<User>(
    Sql.Select("Id", "Name", "Email")
       .From("Users")
       .Where(SqlExpression.Equal("IsActive", 1))
);
```

### Single result

```csharp
var user = connection.QueryFirst<User>(
    Sql.Select("Id", "Name", "Email")
       .From("Users")
       .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
);
```

Use `QueryFirstOrDefault<T>` when the row might not exist, and `QuerySingle<T>` / `QuerySingleOrDefault<T>` when exactly one row is expected.

### Method summary

| Method | Returns | Behaviour |
|---|---|---|
| `Query<T>` | `IEnumerable<T>` | All matching rows |
| `QueryFirst<T>` | `T` | First row; throws if empty |
| `QueryFirstOrDefault<T>` | `T?` | First row or default |
| `QuerySingle<T>` | `T` | Exactly one row; throws if zero or more than one |
| `QuerySingleOrDefault<T>` | `T?` | Zero or one row; throws if more than one |

## Executing statements

`Execute` returns the number of rows affected:

```csharp
// INSERT
int inserted = connection.Execute(
    Sql.Insert()
       .Into("Users", "UserName", "Email")
       .Values("UserName" + (SqlConstant)"jdoe", "Email" + (SqlConstant)"j@example.com")
);

// UPDATE
int updated = connection.Execute(
    Sql.Update("Users")
       .Set("Email", "Email" + (SqlConstant)"new@example.com")
       .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)7))
);

// DELETE
int deleted = connection.Execute(
    Sql.Delete()
       .From("Sessions")
       .Where(SqlExpression.Equal("UserId", "UserId" + (SqlConstant)7))
);
```

## Scalar queries

```csharp
int count = connection.ExecuteScalar<int>(
    Sql.Select(SqlAggregate.Count("Id")).From("Users")
);
```

## ExecuteReader

For advanced scenarios where you need direct access to the `IDataReader`:

```csharp
using var reader = connection.ExecuteReader(
    Sql.Select("Id", "Name", "Email")
       .From("Users")
       .Where(SqlExpression.Equal("IsActive", 1))
);

while (reader.Read())
{
    Console.WriteLine($"{reader["Id"]}: {reader["Name"]}");
}
```

## Async variants

Every method has an `async` counterpart:

```csharp
var users = await connection.QueryAsync<User>(
    Sql.Select("Id", "Name")
       .From("Users")
       .Where(SqlExpression.Equal("IsActive", 1))
);

var user = await connection.QueryFirstOrDefaultAsync<User>(
    Sql.Select("Id", "Name")
       .From("Users")
       .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
);

int affected = await connection.ExecuteAsync(
    Sql.Delete().From("TempData")
);

int count = await connection.ExecuteScalarAsync<int>(
    Sql.Select(SqlAggregate.Count("Id")).From("Users")
);
```

## Multi-map queries

Map rows across multiple tables into a combined result using `Query<TFirst, TSecond, TReturn>`:

```csharp
SqlTable u = new("Users", "u");
SqlTable p = new("Profiles", "p");

var users = connection.Query<User, Profile, User>(
    Sql.Select(u + "Id", u + "Name", p + "Bio", p + "AvatarUrl")
       .From(u)
       .LeftJoin(p).On(SqlExpression.Equal(u + "Id", p + "UserId")),
    (user, profile) =>
    {
        user.Profile = profile;
        return user;
    },
    splitOn: "Bio"
);
```

Overloads with three and four type parameters are also available for queries joining more tables.

## Multiple result sets

Use `QueryMultiple` when a batch or stored procedure returns more than one result set:

```csharp
using var multi = connection.QueryMultiple(
    Sql.Batch(
        Sql.Select("Id", "Name").From("Users"),
        Sql.Select("Id", "Title").From("Posts")
    )
);

var users = multi.Read<User>();
var posts = multi.Read<Post>();
```

## Transactions

All methods accept an optional `IDbTransaction`:

```csharp
using var transaction = connection.BeginTransaction();

connection.Execute(
    Sql.Insert().Into("Orders", "CustomerId", "Total").Values(1, 99.99m),
    transaction: transaction
);

connection.Execute(
    Sql.Update("Inventory").Set("Stock", 9).Where(SqlExpression.Equal("ProductId", 5)),
    transaction: transaction
);

transaction.Commit();
```

## ToCommandDefinition

For fine-grained control, convert a Siqqle statement to a Dapper `CommandDefinition` directly:

```csharp
CommandDefinition cmd = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("IsActive", 1))
    .ToCommandDefinition();

var users = connection.Query<User>(cmd);
```

The `ToCommandDefinition` extension is available on both `ISqlSyntaxEnd<T>` and `SqlStatement` types. It accepts optional `transaction`, `commandTimeout`, `commandType`, `flags`, and `cancellationToken` parameters.

## How parameters work

`Siqqle.Dapper` calls `ToSql()` with a parameter callback internally. Each `SqlParameter` node in the expression tree is added to Dapper's `DynamicParameters`. This means:

- `SqlParameter` values are passed as bound parameters, **never** embedded in the SQL string.
- You can also pass an additional `param` object (anonymous or typed) alongside the Siqqle statement for any extra parameters Dapper should bind.
- `DateOnly` values are automatically converted to `DateTime` with `DbType.Date` for maximum driver compatibility.
- When a `SqlParameter` has a `DbType` set, it is forwarded to Dapper so the database driver uses the correct type mapping.

```csharp
// Combining Siqqle parameters with an extra Dapper param object
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("TenantId", "TenantId" + (SqlConstant)1));

var users = connection.Query<User>(sql, param: new { ExtraFilter = "something" });
```

## Using a dialect with Dapper

Every `Siqqle.Dapper` extension method accepts an optional `SqlDialect dialect` parameter. When omitted, the process-wide default (`SqlDialect.Current`) is used.

### Per-query dialect

Pass the dialect directly to any Dapper extension method:

```csharp
var dialect = new PostgreSqlDialect();

var users = await connection.QueryAsync<User>(
    Sql.Select("Id", "Name").From("Users"),
    dialect: dialect
);

await connection.ExecuteAsync(
    Sql.Insert("Users").Values("Name" + (SqlConstant)"Alice"),
    dialect: dialect
);
```

### Injected dialect

Register the dialect as a singleton and inject it into your repositories:

```csharp
// Program.cs
services.AddSingleton<PostgreSqlDialect>();
```

```csharp
class UserRepository(PostgreSqlDialect dialect, IDbConnection connection)
{
    public async Task<IEnumerable<User>> GetAll() =>
        await connection.QueryAsync<User>(
            Sql.Select("Id", "Name").From("Users"),
            dialect: dialect
        );
}
```

### Global dialect

Alternatively, set a process-wide default at startup so all calls use it automatically:

```csharp
// Program.cs — before any queries
#pragma warning disable CS0618
SqlDialect.UseDialect(new PostgreSqlDialect());
#pragma warning restore CS0618
```

!!! warning
    `UseDialect` is **not thread-safe**. Only call it once during single-threaded startup. Prefer the per-query or injected approaches above.

### `ToCommandDefinition` with a dialect

The `ToCommandDefinition` method also accepts a dialect:

```csharp
var cmd = Sql.Select("Id", "Name")
    .From("Users")
    .ToCommandDefinition(dialect: new PostgreSqlDialect());

var users = await connection.QueryAsync<User>(cmd);
```
