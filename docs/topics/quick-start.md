# Quick Start

This page walks through the most common operations in Siqqle. Every query starts with the `Sql` static class, which provides factory methods for all statement types.

## Setup

Make sure you have the `Siqqle` package installed (see [Installation](installation.md)) and add the core namespaces:

```csharp
using Siqqle;
using Siqqle.Expressions;
```

## Your first query

```csharp
var sql = Sql
    .Select("Id", "UserName", "Email", "Age")
    .From("Users")
    .Where(SqlExpression.GreaterThanOrEqual("Age", 30))
    .OrderBy("UserName")
    .ToSql();
```

```sql
SELECT [Id], [UserName], [Email], [Age] FROM [Users] WHERE [Age] >= 30 ORDER BY [UserName] ASC
```

Column names passed as strings are implicitly converted to `SqlColumn` instances, and literal values like `30` are implicitly converted to `SqlConstant`. See [Selecting Data](selecting.md) for more options.

## Joining tables

Use `SqlTable` with aliases and chain `.InnerJoin()` or `.LeftJoin()` calls:

```csharp
SqlTable u = new("Users", "u");
SqlTable o = new("Orders", "o");

var sql = Sql
    .Select(u + "UserName", o + "OrderDate", o + "Total")
    .From(u)
    .InnerJoin(o).On(SqlExpression.Equal(u + "Id", o + "UserId"))
    .Where(SqlExpression.GreaterThan(o + "Total", 100))
    .OrderBy(o + "OrderDate", SqlSortOrder.Descending)
    .ToSql();
```

```sql
SELECT [u].[UserName], [o].[OrderDate], [o].[Total]
FROM [Users] [u]
INNER JOIN [Orders] [o] ON [u].[Id] = [o].[UserId]
WHERE [o].[Total] > 100
ORDER BY [o].[OrderDate] DESC
```

See [Joins](joins.md) for all join types.

## Aggregating data

```csharp
var sql = Sql
    .Select(
        "Department",
        SqlAggregate.Count("Id", "Headcount"),
        SqlAggregate.Average("Salary", "AvgSalary")
    )
    .From("Employees")
    .GroupBy("Department")
    .Having(SqlExpression.GreaterThan(SqlAggregate.Count("Id"), 5))
    .ToSql();
```

```sql
SELECT [Department], COUNT([Id]) AS [Headcount], AVG([Salary]) AS [AvgSalary]
FROM [Employees]
GROUP BY ([Department]) HAVING COUNT([Id]) > 5
```

See [Grouping & Aggregates](grouping.md) for the full set of aggregate functions.

## Inserting data

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

See [Inserting Data](insert.md) for multi-row inserts and parameterised values.

## Updating data

```csharp
var sql = Sql
    .Update("Users")
    .Set("Email", "jane.new@example.com")
    .Where(SqlExpression.Equal("Id", 42))
    .ToSql();
```

```sql
UPDATE [Users] SET [Email] = 'jane.new@example.com' WHERE [Id] = 42
```

See [Updating Data](update.md) for more options.

## Deleting data

```csharp
var sql = Sql
    .Delete()
    .From("Users")
    .Where(SqlExpression.Equal("Id", 42))
    .ToSql();
```

```sql
DELETE FROM [Users] WHERE [Id] = 42
```

See [Deleting Data](delete.md) for compound conditions and parameterised deletes.

## Using parameters

Use named parameters instead of literal values for production queries. Create a parameter by combining a parameter name with a `SqlConstant`:

```csharp
var sql = Sql
    .Select("Id", "UserName")
    .From("Users")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
    .ToSql();
```

```sql
SELECT [Id], [UserName] FROM [Users] WHERE [Id] = @UserId
```

The parameter value (`42`) is extracted via a callback or automatically when using [Dapper integration](dapper.md). See [Parameters](parameters.md) for full details.

## Choosing a SQL dialect

By default Siqqle produces SQL with `[square bracket]` identifier quoting. Pass a dialect to `ToSql()` to target a specific database:

```csharp
var query = Sql.Select("Id", "Name").From("Users");
```

=== "Default"

    ```csharp
    query.ToSql();
    ```
    ```sql
    SELECT [Id], [Name] FROM [Users]
    ```

=== "PostgreSQL"

    ```csharp
    query.ToSql(new PostgreSqlDialect());
    ```
    ```sql
    SELECT "Id", "Name" FROM "Users"
    ```

=== "MySQL"

    ```csharp
    query.ToSql(new MySqlDialect());
    ```
    ```sql
    SELECT `Id`, `Name` FROM `Users`
    ```

See [SQL Dialects](dialects.md) for details on all available dialects.

## Using with Dapper

With the `Siqqle.Dapper` package, you can pass Siqqle statements directly to `IDbConnection` extension methods. Parameters are extracted automatically:

```csharp
using Siqqle.Dapper;

var users = connection.Query<User>(
    Sql.Select("Id", "Name", "Email")
       .From("Users")
       .Where(SqlExpression.Equal("IsActive", 1))
       .OrderBy("Name")
);
```

See [Dapper Integration](dapper.md) for the full API.

## Next steps

| Topic | What you'll learn |
|---|---|
| [Selecting Data](selecting.md) | Columns, aliases, functions, `DISTINCT`, subquery sources |
| [Filtering](filtering.md) | `WHERE`, comparisons, `AND`/`OR`, `IN`, `BETWEEN`, `LIKE`, `IS NULL` |
| [Joins](joins.md) | `INNER`, `LEFT`, `RIGHT` joins, multi-table queries |
| [Grouping & Aggregates](grouping.md) | `GROUP BY`, `HAVING`, `COUNT`, `SUM`, `AVG`, `MIN`, `MAX` |
| [Ordering & Paging](ordering-paging.md) | `ORDER BY`, `LIMIT`/`OFFSET`, dialect-specific paging |
| [Expressions](expressions.md) | `CASE`/`WHEN`, `CAST`, arithmetic, concatenation, stored procedures |
| [Parameters](parameters.md) | Named parameters, `DbType`, ADO.NET integration |
| [SQL Dialects](dialects.md) | Dialect differences, custom dialects, DI integration |
| [Dapper Integration](dapper.md) | Query, Execute, transactions, multi-map, async |
