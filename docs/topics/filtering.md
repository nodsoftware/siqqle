# Filtering

The `WHERE` clause is added via `.Where()` and accepts any `SqlExpression`. All comparison and logical factory methods live on the `SqlExpression` static class.

## Comparison operators

| Method | SQL operator |
|---|---|
| `SqlExpression.Equal(col, val)` | `=` |
| `SqlExpression.NotEqual(col, val)` | `<>` |
| `SqlExpression.GreaterThan(col, val)` | `>` |
| `SqlExpression.GreaterThanOrEqual(col, val)` | `>=` |
| `SqlExpression.LessThan(col, val)` | `<` |
| `SqlExpression.LessThanOrEqual(col, val)` | `<=` |

```csharp
var sql = Sql
    .Select("Id", "UserName", "Email", "Age")
    .From("Users")
    .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
    .ToSql();
```

```sql
SELECT [Id], [UserName], [Email], [Age] FROM [Users] WHERE [Age] >= 18
```

### Column-to-column comparison

Both sides of a comparison can be columns — useful for self-referencing checks:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Products")
    .Where(SqlExpression.GreaterThan((SqlColumn)"SalePrice", (SqlColumn)"CostPrice"))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Products] WHERE [SalePrice] > [CostPrice]
```

### NULL values

Passing `null` as the value automatically becomes `NULL`:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("DeletedAt", null))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [DeletedAt] = NULL
```

!!! tip
    To test for `NULL` membership use `SqlExpression.IsNull()` instead — see [IS NULL / IS NOT NULL](#is-null--is-not-null) below.

## AND / OR

Combine expressions with `SqlExpression.And()` and `SqlExpression.Or()`:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(
        SqlExpression.And(
            SqlExpression.Between("Age", 18, 65),
            SqlExpression.Equal("Status", "Active")
        )
    )
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE ([Age] BETWEEN 18 AND 65) AND ([Status] = 'Active')
```

```csharp
var sql = Sql
    .Select("Name")
    .From("Products")
    .Where(
        SqlExpression.Or(
            SqlExpression.Between("Price", 10, 50),
            SqlExpression.Between("Price", 100, 200)
        )
    )
    .ToSql();
```

```sql
SELECT [Name] FROM [Products] WHERE ([Price] BETWEEN 10 AND 50) OR ([Price] BETWEEN 100 AND 200)
```

### Nesting AND / OR

Combine `And` and `Or` at any depth to build complex predicates:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(
        SqlExpression.And(
            SqlExpression.Equal("IsActive", true),
            SqlExpression.Or(
                SqlExpression.Equal("Role", "Admin"),
                SqlExpression.And(
                    SqlExpression.Equal("Role", "Editor"),
                    SqlExpression.GreaterThanOrEqual("Experience", 5)
                )
            )
        )
    )
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users]
WHERE ([IsActive] = True) AND (([Role] = 'Admin') OR (([Role] = 'Editor') AND ([Experience] >= 5)))
```

## BETWEEN

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Between("Age", 18, 65))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Age] BETWEEN 18 AND 65
```

Use parameters for the bounds to avoid injection risk:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(
        SqlExpression.Between(
            "Age",
            "MinAge" + (SqlConstant)18,
            "MaxAge" + (SqlConstant)65
        )
    )
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Age] BETWEEN @MinAge AND @MaxAge
```

## IN / NOT IN

### Value list

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.In("Status", "Active", "Pending", "Approved"))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Status] IN ('Active', 'Pending', 'Approved')
```

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.NotIn("Status", "Deleted", "Suspended", "Banned"))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Status] NOT IN ('Deleted', 'Suspended', 'Banned')
```

### Subquery

```csharp
var subquery = new SqlSubquery(
    Sql.Select("Id").From("Categories").Where(SqlExpression.Equal("IsActive", 1))
);

var sql = Sql
    .Select("Name")
    .From("Products")
    .Where(SqlExpression.In("CategoryId", subquery))
    .ToSql();
```

```sql
SELECT [Name] FROM [Products] WHERE [CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [IsActive] = 1)
```

See [Subqueries](subqueries.md) for more patterns including `NOT IN` with subqueries.

## LIKE / NOT LIKE

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Like(new SqlColumn("Name"), new SqlConstant("John%")))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Name] LIKE 'John%'
```

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.NotLike(new SqlColumn("Name"), new SqlConstant("%Admin%")))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Name] NOT LIKE '%Admin%'
```

Works with table-qualified columns too:

```csharp
SqlTable users = new("Users", "u");

var sql = Sql
    .Select(users + "Id", users + "Name")
    .From(users)
    .Where(SqlExpression.Like(users + "Name", new SqlConstant("A%")))
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[Name] FROM [Users] [u] WHERE [u].[Name] LIKE 'A%'
```

## IS NULL / IS NOT NULL

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.IsNull(new SqlColumn("Email")))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE [Email] IS NULL
```

```csharp
var sql = Sql
    .Select("p.Name")
    .From(new SqlTable("Products", "p"))
    .Where(SqlExpression.IsNotNull(new SqlTable("Products", "p") + "Description"))
    .ToSql();
```

```sql
SELECT [p].[Name] FROM [Products] [p] WHERE [p].[Description] IS NOT NULL
```

## EXISTS / NOT EXISTS

Test whether a correlated subquery returns any rows. Reference columns from the outer query using dotted names:

```csharp
var subquery = new SqlSubquery(
    Sql.Select(1)
        .From("Orders")
        .Where(
            SqlExpression.Equal(new SqlColumn("Orders.UserId"), new SqlColumn("Users.Id"))
        )
);

var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Exists(subquery))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])
```

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.NotExists(subquery))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users] WHERE NOT EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])
```

See [Subqueries](subqueries.md) for more correlated subquery patterns.

## Summary

| Expression | SQL |
|---|---|
| `Equal(col, val)` | `col = val` |
| `NotEqual(col, val)` | `col <> val` |
| `GreaterThan(col, val)` | `col > val` |
| `GreaterThanOrEqual(col, val)` | `col >= val` |
| `LessThan(col, val)` | `col < val` |
| `LessThanOrEqual(col, val)` | `col <= val` |
| `Between(col, min, max)` | `col BETWEEN min AND max` |
| `In(col, values...)` | `col IN (...)` |
| `NotIn(col, values...)` | `col NOT IN (...)` |
| `Like(col, pattern)` | `col LIKE pattern` |
| `NotLike(col, pattern)` | `col NOT LIKE pattern` |
| `IsNull(col)` | `col IS NULL` |
| `IsNotNull(col)` | `col IS NOT NULL` |
| `Exists(subquery)` | `EXISTS (subquery)` |
| `NotExists(subquery)` | `NOT EXISTS (subquery)` |
| `And(left, right)` | `(left) AND (right)` |
| `Or(left, right)` | `(left) OR (right)` |
