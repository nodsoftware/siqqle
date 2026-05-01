# Ordering and Paging

## ORDER BY

Add one or more sort expressions with `.OrderBy()`. Columns sort ascending by default.

```csharp
var sql = Sql
    .Select("Id", "UserName", "Email", "Age")
    .From("Users")
    .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
    .OrderBy("UserName")
    .ToSql();
```

```sql
SELECT [Id], [UserName], [Email], [Age] FROM [Users] WHERE [Age] >= 18 ORDER BY [UserName] ASC
```

## Descending order

Pass `SqlSortOrder.Descending` as the second argument:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .OrderBy("Id", SqlSortOrder.Descending)
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] ORDER BY [Id] DESC
```

## Multiple columns

Chain multiple `.OrderBy()` calls to sort by several columns:

```csharp
SqlTable u = new("Users", "u");

var sql = Sql
    .Select(u + "Id", u + "UserName")
    .From(u)
    .OrderBy(u + "UserName")
    .OrderBy(u + "Id", SqlSortOrder.Descending)
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[UserName] FROM [Users] [u] ORDER BY [u].[UserName] ASC, [u].[Id] DESC
```

## Ordering by aggregate

You can order by an aggregate function — useful after `GROUP BY`:

```csharp
var sql = Sql
    .Select("Department", SqlAggregate.Count("Id", "Headcount"))
    .From("Employees")
    .GroupBy("Department")
    .OrderBy(SqlAggregate.Count("Id"), SqlSortOrder.Descending)
    .ToSql();
```

```sql
SELECT [Department], COUNT([Id]) AS [Headcount]
FROM [Employees]
GROUP BY ([Department])
ORDER BY COUNT([Id]) DESC
```

## DISTINCT

Call `.Distinct()` immediately after `.Select()`:

```csharp
var sql = Sql
    .Select("City", "Country")
    .Distinct()
    .From("Users")
    .ToSql();
```

```sql
SELECT DISTINCT [City], [Country] FROM [Users]
```

`.Distinct()` works together with `WHERE`, `ORDER BY`, and `LIMIT`:

```csharp
var sql = Sql
    .Select("City")
    .Distinct()
    .From("Users")
    .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
    .OrderBy("City")
    .ToSql();
```

```sql
SELECT DISTINCT [City] FROM [Users] WHERE [Age] >= 18 ORDER BY [City] ASC
```

## Paging (LIMIT / OFFSET)

Add `.Limit(offset, count)` after `.OrderBy()` to page through results. The SQL produced depends on the active dialect.

### Skip and take

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .OrderBy("Id")
    .Limit(20, 10)   // skip 20 rows, take 10
    .ToSql();
```

=== "Default / SQL Server"

    ```sql
    SELECT [Id], [Name] FROM [Users] ORDER BY [Id] ASC OFFSET 20 ROWS FETCH FIRST 10 ROWS ONLY
    ```

=== "PostgreSQL"

    ```sql
    SELECT "Id", "Name" FROM "Users" ORDER BY "Id" ASC LIMIT 10 OFFSET 20
    ```

=== "MySQL"

    ```sql
    SELECT `Id`, `Name` FROM `Users` ORDER BY `Id` ASC LIMIT 20, 10
    ```

### Take first N rows

Pass `null` for the offset to take from the beginning:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .OrderBy("Id")
    .Limit(null, 5)   // take 5 rows from the beginning
    .ToSql();
```

=== "Default / SQL Server"

    ```sql
    SELECT [Id], [Name] FROM [Users] ORDER BY [Id] ASC OFFSET 0 ROWS FETCH FIRST 5 ROWS ONLY
    ```

=== "PostgreSQL"

    ```sql
    SELECT "Id", "Name" FROM "Users" ORDER BY "Id" ASC LIMIT 5
    ```

=== "MySQL"

    ```sql
    SELECT `Id`, `Name` FROM `Users` ORDER BY `Id` ASC LIMIT 5
    ```

### Practical pagination example

A typical "page N of results" pattern:

```csharp
int pageNumber = 3;
int pageSize = 25;
int offset = (pageNumber - 1) * pageSize;

var sql = Sql
    .Select("Id", "Name", "Email")
    .From("Users")
    .Where(SqlExpression.Equal("IsActive", 1))
    .OrderBy("Name")
    .Limit(offset, pageSize)
    .ToSql();
```

=== "Default / SQL Server"

    ```sql
    SELECT [Id], [Name], [Email] FROM [Users] WHERE [IsActive] = 1 ORDER BY [Name] ASC OFFSET 50 ROWS FETCH FIRST 25 ROWS ONLY
    ```

=== "PostgreSQL"

    ```sql
    SELECT "Id", "Name", "Email" FROM "Users" WHERE "IsActive" = 1 ORDER BY "Name" ASC LIMIT 25 OFFSET 50
    ```

!!! note
    SQL Server requires `ORDER BY` to be present when using `OFFSET`/`FETCH`. The `SqlServerDialect` enforces this by automatically supplying `OFFSET 0` when no offset is given.

## Dialect paging summary

| Dialect | Syntax |
|---|---|
| Default / SQL Server | `OFFSET n ROWS FETCH FIRST n ROWS ONLY` |
| PostgreSQL | `LIMIT n OFFSET n` |
| MySQL | `LIMIT offset, count` |

See [SQL Dialects](dialects.md) for more dialect-specific behaviour.
