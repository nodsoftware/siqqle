# Subqueries

A `SqlSubquery` wraps a `SELECT` statement so it can be used wherever a table expression or a scalar value is expected.

## Creating a subquery

Wrap any `SELECT` statement (or `ISqlSyntaxEnd<SqlSelect>`) in a `SqlSubquery`:

```csharp
// From an ISqlSyntaxEnd<SqlSelect> (the fluent builder result)
var subquery = new SqlSubquery(
    Sql.Select("Id").From("Users").Where(SqlExpression.Equal("IsActive", 1))
);

// From a materialised SqlSelect
SqlSelect select = Sql.Select("Id").From("Users").Go();
var subquery2 = new SqlSubquery(select);
```

## Subquery in WHERE with IN

The most common use — filter rows where a column matches values from another table:

```csharp
var subquery = new SqlSubquery(
    Sql.Select("Id")
       .From("Categories")
       .Where(SqlExpression.Equal("IsActive", 1))
);

var sql = Sql
    .Select("Name")
    .From("Products")
    .Where(SqlExpression.In("CategoryId", subquery))
    .ToSql();
```

```sql
SELECT [Name] FROM [Products]
WHERE [CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [IsActive] = 1)
```

## Subquery in WHERE with NOT IN

```csharp
var subquery = new SqlSubquery(Sql.Select("UserId").From("BlockedUsers"));

var sql = Sql
    .Select("Name")
    .From("Users")
    .Where(SqlExpression.NotIn("Id", subquery))
    .ToSql();
```

```sql
SELECT [Name] FROM [Users] WHERE [Id] NOT IN (SELECT [UserId] FROM [BlockedUsers])
```

## Subquery in WHERE with EXISTS

Use `SqlExpression.Exists()` or `SqlExpression.NotExists()` to test whether a correlated subquery returns any rows. Reference columns from the outer query using dotted names:

```csharp
var subquery = new SqlSubquery(
    Sql.Select(1)
       .From("Orders")
       .Where(SqlExpression.Equal(
           new SqlColumn("Orders.UserId"),
           new SqlColumn("Users.Id")
       ))
);

var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Exists(subquery))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users]
WHERE EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])
```

### NOT EXISTS

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.NotExists(subquery))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Users]
WHERE NOT EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])
```

!!! tip
    `EXISTS` / `NOT EXISTS` is often more efficient than `IN` / `NOT IN` for correlated subqueries, especially when the subquery table is large.

## Subquery with IS NOT NULL

A subquery can include any filter condition — for example, `IS NOT NULL`:

```csharp
var subquery = new SqlSubquery(
    Sql.Select("Id")
       .From("Categories")
       .Where(SqlExpression.IsNotNull(new SqlColumn("ParentId")))
);

var sql = Sql
    .Select("Name")
    .From("Products")
    .Where(SqlExpression.In("CategoryId", subquery))
    .ToSql();
```

```sql
SELECT [Name] FROM [Products]
WHERE [CategoryId] IN (SELECT [Id] FROM [Categories] WHERE [ParentId] IS NOT NULL)
```

## Subquery as FROM source

A subquery used as the `FROM` table **must** have an alias. Add one with the `+` operator on the `SqlSelect`:

```csharp
var inner = Sql
    .Select("UserId", "SUM(Amount) AS Total")
    .From("Orders")
    .GroupBy("UserId")
    .Go() + "totals";          // assigns alias "totals"

var sql = Sql
    .Select("totals.UserId", "totals.Total")
    .From(inner)
    .Where(SqlExpression.GreaterThan("totals.Total", 500))
    .ToSql();
```

```sql
SELECT [totals].[UserId], [totals].[Total]
FROM (SELECT [UserId], SUM(Amount) AS Total FROM [Orders] GROUP BY ([UserId])) [totals]
WHERE [totals].[Total] > 500
```

!!! note
    Passing a `SqlSubquery` without an alias to `.From()` throws an `ArgumentException`.

## Nested subqueries

Subqueries can be nested — a subquery's `WHERE` clause can itself contain another subquery:

```csharp
var innerSub = new SqlSubquery(
    Sql.Select("Id").From("Regions").Where(SqlExpression.Equal("Country", "US"))
);

var outerSub = new SqlSubquery(
    Sql.Select("Id")
       .From("Stores")
       .Where(SqlExpression.In("RegionId", innerSub))
);

var sql = Sql
    .Select("Name", "Total")
    .From("Sales")
    .Where(SqlExpression.In("StoreId", outerSub))
    .ToSql();
```

```sql
SELECT [Name], [Total] FROM [Sales]
WHERE [StoreId] IN (SELECT [Id] FROM [Stores] WHERE [RegionId] IN (SELECT [Id] FROM [Regions] WHERE [Country] = 'US'))
```

## Subquery with parameters

Subqueries support named parameters just like top-level queries:

```csharp
var subquery = new SqlSubquery(
    Sql.Select("Id")
       .From("Departments")
       .Where(SqlExpression.Equal("Name", "DeptName" + (SqlConstant)"Engineering"))
);

var sql = Sql
    .Select("Name", "Email")
    .From("Employees")
    .Where(SqlExpression.In("DepartmentId", subquery))
    .ToSql();
```

```sql
SELECT [Name], [Email] FROM [Employees]
WHERE [DepartmentId] IN (SELECT [Id] FROM [Departments] WHERE [Name] = @DeptName)
```
