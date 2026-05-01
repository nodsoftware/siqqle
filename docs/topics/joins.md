# Joins

Siqqle supports all standard join types. Joins are added as a chain of `.XxxJoin(table).On(predicate)` calls after `.From()`.

## Join types

| Method | SQL |
|---|---|
| `.Join(table)` | `JOIN` |
| `.InnerJoin(table)` | `INNER JOIN` |
| `.LeftJoin(table)` | `LEFT JOIN` |
| `.RightJoin(table)` | `RIGHT JOIN` |

All join methods accept either a `SqlTable` (with or without an alias) or a plain string (implicitly converted to `SqlTable`).

## Inner join

```csharp
SqlTable users = new("Users", "u");
SqlTable profiles = new("Profiles", "p");

var sql = Sql
    .Select(users + "Id", users + "UserName", profiles + "Email", profiles + "Age")
    .From(users)
    .InnerJoin(profiles)
    .On(SqlExpression.Equal(users + "Id", profiles + "UserId"))
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[UserName], [p].[Email], [p].[Age]
FROM [Users] [u]
INNER JOIN [Profiles] [p] ON [u].[Id] = [p].[UserId]
```

## Left join

A `LEFT JOIN` returns all rows from the left table and matching rows from the right table. Unmatched right-side columns are `NULL`.

```csharp
SqlTable u = new("Users", "u");
SqlTable p = new("Profiles", "p");

var sql = Sql
    .Select(u + "Id", u + "UserName", p + "Email", p + "Age")
    .From(u)
    .LeftJoin(p)
    .On(SqlExpression.Equal(u + "Id", p + "UserId"))
    .Where(SqlExpression.GreaterThanOrEqual(p + "Age", 30))
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[UserName], [p].[Email], [p].[Age]
FROM [Users] [u]
LEFT JOIN [Profiles] [p] ON [u].[Id] = [p].[UserId]
WHERE [p].[Age] >= 30
```

## Right join

A `RIGHT JOIN` returns all rows from the right table and matching rows from the left table:

```csharp
SqlTable e = new("Employees", "e");
SqlTable d = new("Departments", "d");

var sql = Sql
    .Select(e + "Name", d + "DepartmentName")
    .From(e)
    .RightJoin(d)
    .On(SqlExpression.Equal(e + "DepartmentId", d + "Id"))
    .ToSql();
```

```sql
SELECT [e].[Name], [d].[DepartmentName]
FROM [Employees] [e]
RIGHT JOIN [Departments] [d] ON [e].[DepartmentId] = [d].[Id]
```

## Chaining multiple joins

```csharp
SqlTable o = new("Orders", "o");
SqlTable c = new("Customers", "c");
SqlTable p = new("Products", "p");
SqlTable oi = new("OrderItems", "oi");

var sql = Sql
    .Select(o + "Id", c + "Name", p + "ProductName")
    .From(o)
    .InnerJoin(c).On(SqlExpression.Equal(o + "CustomerId", c + "Id"))
    .InnerJoin(oi).On(SqlExpression.Equal(o + "Id", oi + "OrderId"))
    .InnerJoin(p).On(SqlExpression.Equal(oi + "ProductId", p + "Id"))
    .Where(SqlExpression.Equal(o + "Status", "Shipped"))
    .ToSql();
```

```sql
SELECT [o].[Id], [c].[Name], [p].[ProductName]
FROM [Orders] [o]
INNER JOIN [Customers] [c] ON [o].[CustomerId] = [c].[Id]
INNER JOIN [OrderItems] [oi] ON [o].[Id] = [oi].[OrderId]
INNER JOIN [Products] [p] ON [oi].[ProductId] = [p].[Id]
WHERE [o].[Status] = 'Shipped'
```

## Compound ON conditions

Use `SqlExpression.And()` to add multiple conditions to the `ON` clause:

```csharp
SqlTable u = new("Users", "u");
SqlTable r = new("UserRoles", "r");

var sql = Sql
    .Select(u + "Name", r + "Role")
    .From(u)
    .InnerJoin(r)
    .On(SqlExpression.And(
        SqlExpression.Equal(u + "Id", r + "UserId"),
        SqlExpression.Equal(r + "IsActive", 1)
    ))
    .ToSql();
```

```sql
SELECT [u].[Name], [r].[Role]
FROM [Users] [u]
INNER JOIN [UserRoles] [r] ON ([u].[Id] = [r].[UserId]) AND ([r].[IsActive] = 1)
```

## Self-join

Join a table to itself by creating two aliases for the same table:

```csharp
SqlTable e = new("Employees", "e");
SqlTable m = new("Employees", "m");

var sql = Sql
    .Select(e + "Name", m + "Name")
    .From(e)
    .LeftJoin(m)
    .On(SqlExpression.Equal(e + "ManagerId", m + "Id"))
    .ToSql();
```

```sql
SELECT [e].[Name], [m].[Name]
FROM [Employees] [e]
LEFT JOIN [Employees] [m] ON [e].[ManagerId] = [m].[Id]
```

## Left anti-join pattern

Combine a `LEFT JOIN` with `IS NULL` to find rows with no related record:

```csharp
SqlTable users = new("Users", "u");
SqlTable orders = new("Orders", "o");

var sql = Sql
    .Select(users + "Name")
    .From(users)
    .LeftJoin(orders)
    .On(SqlExpression.Equal(users + "Id", orders + "UserId"))
    .Where(SqlExpression.IsNull(orders + "Id"))
    .ToSql();
```

```sql
SELECT [u].[Name]
FROM [Users] [u]
LEFT JOIN [Orders] [o] ON [u].[Id] = [o].[UserId]
WHERE [o].[Id] IS NULL
```

!!! tip
    This pattern is equivalent to `NOT EXISTS` but can sometimes perform better depending on the database engine. See [Filtering — EXISTS / NOT EXISTS](filtering.md#exists--not-exists) for the subquery alternative.

## Filtering on join result

You can chain `.Where()` after any join to filter the combined result set:

```csharp
SqlTable users = new("Users", "u");
SqlTable orders = new("Orders", "o");

var sql = Sql
    .Select(users + "Name")
    .From(users)
    .Join(orders)
    .On(SqlExpression.Equal(users + "Id", orders + "UserId"))
    .Where(SqlExpression.IsNull(orders + "ShippedDate"))
    .ToSql();
```

```sql
SELECT [u].[Name]
FROM [Users] [u]
JOIN [Orders] [o] ON [u].[Id] = [o].[UserId]
WHERE [o].[ShippedDate] IS NULL
```

## Schema-qualified table names

Pass a dotted name to use schema-qualified tables. Siqqle parses the segments and quotes each part:

```csharp
SqlTable users = new("dbo.Users", "u");

var sql = Sql
    .Select(users + "Id", users + "Name")
    .From(users)
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[Name] FROM [dbo].[Users] [u]
```
