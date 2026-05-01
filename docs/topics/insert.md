# Inserting Data

`INSERT` statements are built with `Sql.Insert()`, followed by `.Into()` and one or more `.Values()` calls.

## Basic insert

```csharp
var sql = Sql
    .Insert()
    .Into("Users", "UserName", "Email")
    .Values("jdoe", "jane.doe@example.com")
    .ToSql();
```

```sql
INSERT INTO [Users] ([UserName], [Email]) VALUES ('jdoe', 'jane.doe@example.com')
```

## Inserting NULL

Use `SqlConstant.Null` to represent a SQL `NULL`:

```csharp
var sql = Sql
    .Insert()
    .Into("Users", "Id", "UserName", "Email")
    .Values(SqlConstant.Null, "jdoe", "jane.doe@example.com")
    .ToSql();
```

```sql
INSERT INTO [Users] ([Id], [UserName], [Email]) VALUES (NULL, 'jdoe', 'jane.doe@example.com')
```

## Multi-row insert

Chain multiple `.Values()` calls to produce a multi-row `INSERT` in a single statement:

```csharp
var sql = Sql
    .Insert()
    .Into("Users", "Id", "UserName", "Email")
    .Values(SqlConstant.Null, "janed", "jane.doe@example.com")
    .Values(SqlConstant.Null, "joed", "joe.doe@example.com")
    .Values(SqlConstant.Null, "jimmyd", "jimmy.doe@example.com")
    .ToSql();
```

```sql
INSERT INTO [Users] ([Id], [UserName], [Email])
VALUES
    (NULL, 'janed', 'jane.doe@example.com'),
    (NULL, 'joed', 'joe.doe@example.com'),
    (NULL, 'jimmyd', 'jimmy.doe@example.com')
```

## Inserting with parameters

Use named parameters to avoid injection risks and pass values through your data access layer:

```csharp
var sql = Sql
    .Insert()
    .Into("Users", "UserName", "Email", "Age")
    .Values(
        "UserName" + (SqlConstant)"jdoe",
        "Email" + (SqlConstant)"jane.doe@example.com",
        "Age" + (SqlConstant)28
    )
    .ToSql(p =>
    {
        // p.ParameterName = "UserName", "Email", "Age"
        // p.Value = the corresponding value
        command.Parameters.AddWithValue(p.ParameterName, p.Value);
    });
```

```sql
INSERT INTO [Users] ([UserName], [Email], [Age]) VALUES (@UserName, @Email, @Age)
```

The callback receives each `SqlParameter` as the expression tree is visited, letting you bind values to your `DbCommand`. See [Parameters](parameters.md) for full details.

## Insert without explicit columns

You can omit the column list by calling `.Into(table)` with only the table name. Values are then inserted positionally in the table's column order:

```csharp
var sql = Sql
    .Insert()
    .Into("Users")
    .Values(SqlConstant.Null, "jdoe", "jane.doe@example.com")
    .ToSql();
```

```sql
INSERT INTO [Users] VALUES (NULL, 'jdoe', 'jane.doe@example.com')
```

!!! warning
    Omitting the column list makes the statement fragile — it will break if the table schema changes. Prefer explicit columns for production code.

## Insert with different value types

Siqqle handles all common .NET types through implicit conversions to `SqlConstant`:

```csharp
var sql = Sql
    .Insert()
    .Into("Products", "Name", "Price", "InStock", "CreatedAt")
    .Values("Widget", 19.99m, true, DateTime.UtcNow)
    .ToSql();
```

```sql
INSERT INTO [Products] ([Name], [Price], [InStock], [CreatedAt])
VALUES ('Widget', 19.99, True, '2026-05-01T12:00:00.0000000Z')
```

## Insert with Dapper

With `Siqqle.Dapper`, parameter binding is automatic:

```csharp
int rows = connection.Execute(
    Sql.Insert()
       .Into("Users", "UserName", "Email")
       .Values("UserName" + (SqlConstant)"jdoe", "Email" + (SqlConstant)"j@example.com")
);
```

See [Dapper Integration](dapper.md) for the full API.
