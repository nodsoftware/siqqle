# Unions

`Sql.Union()` combines two or more `SELECT` statements into a `UNION` query, which eliminates duplicate rows.

## Basic union

```csharp
var sql = Sql.Union(
    Sql.Select("Name").From("Suppliers").Go(),
    Sql.Select("Name").From("Customers").Go()
).ToSql();
```

```sql
SELECT [Name] FROM [Suppliers]
UNION
SELECT [Name] FROM [Customers]
```

## Union with conditions

Each member query can have its own `WHERE` clause:

```csharp
var activeUsers = Sql
    .Select("Id", "Name", "Email")
    .From("Users")
    .Where(SqlExpression.Equal("IsActive", 1));

var admins = Sql
    .Select("Id", "Name", "Email")
    .From("Admins")
    .Where(SqlExpression.Equal("IsActive", 1));

var sql = Sql.Union(activeUsers, admins).ToSql();
```

```sql
SELECT [Id], [Name], [Email] FROM [Users] WHERE [IsActive] = 1
UNION
SELECT [Id], [Name], [Email] FROM [Admins] WHERE [IsActive] = 1
```

!!! tip
    `Sql.Union()` accepts both `SqlSelect` objects (via `.Go()`) and `ISqlSyntaxEnd<SqlSelect>` objects (the direct result of the fluent builder). You can mix both forms.

## Three or more queries

```csharp
var sql = Sql.Union(
    Sql.Select("Id", "Name").From("Employees").Go(),
    Sql.Select("Id", "Name").From("Contractors").Go(),
    Sql.Select("Id", "Name").From("Consultants").Go()
).ToSql();
```

```sql
SELECT [Id], [Name] FROM [Employees]
UNION
SELECT [Id], [Name] FROM [Contractors]
UNION
SELECT [Id], [Name] FROM [Consultants]
```

## Union with parameters

Parameters in union queries work the same way as in regular queries:

```csharp
var sql = Sql.Union(
    Sql.Select("Id", "Name")
       .From("Users")
       .Where(SqlExpression.Equal("Role", "RoleFilter" + (SqlConstant)"Admin")),
    Sql.Select("Id", "Name")
       .From("Users")
       .Where(SqlExpression.Equal("Role", "RoleFilter2" + (SqlConstant)"SuperAdmin"))
).ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Role] = @RoleFilter
UNION
SELECT [Id], [Name] FROM [Users] WHERE [Role] = @RoleFilter2
```

## Union with different dialects

Like any statement, unions respect the active dialect:

```csharp
var sql = Sql.Union(
    Sql.Select("Id", "Name").From("Users").Go(),
    Sql.Select("Id", "Name").From("Admins").Go()
).ToSql(new PostgreSqlDialect());
```

```sql
SELECT "Id", "Name" FROM "Users"
UNION
SELECT "Id", "Name" FROM "Admins"
```

## Union with Dapper

Use `ExecuteReader` or `QueryMultiple` to process union results with Dapper:

```csharp
var results = connection.Query<Person>(
    Sql.Union(
        Sql.Select("Id", "Name").From("Users").Go(),
        Sql.Select("Id", "Name").From("Admins").Go()
    )
);
```

See [Dapper Integration](dapper.md) for more details.

!!! note
    `Sql.Union()` requires at least one statement. Passing an empty collection throws `ArgumentException`.
