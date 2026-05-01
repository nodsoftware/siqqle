# Grouping and Aggregates

## GROUP BY

Add a `GROUP BY` clause with `.GroupBy()`. You can group by one or more columns:

```csharp
var sql = Sql
    .Select(
        new SqlColumn("CreatedBy", "Author"),
        SqlAggregate.Count("Id", "Count")
    )
    .From("Post")
    .GroupBy("CreatedBy")
    .ToSql();
```

```sql
SELECT [CreatedBy] AS [Author], COUNT([Id]) AS [Count] FROM [Post] GROUP BY ([CreatedBy])
```

### Multiple GROUP BY columns

Chain `.GroupBy()` calls to group by multiple columns:

```csharp
var sql = Sql
    .Select("Country", "City", SqlAggregate.Count("Id", "Count"))
    .From("Users")
    .GroupBy("Country")
    .GroupBy("City")
    .ToSql();
```

```sql
SELECT [Country], [City], COUNT([Id]) AS [Count] FROM [Users] GROUP BY ([Country]), ([City])
```

### Table-qualified GROUP BY

With table aliases:

```csharp
SqlTable profiles = new("Profile", "p");

var sql = Sql
    .Select(
        profiles + "Age",
        SqlAggregate.Count(profiles + "Id", "Count")
    )
    .From(profiles)
    .GroupBy(profiles + "Age")
    .ToSql();
```

```sql
SELECT [p].[Age], COUNT([p].[Id]) AS [Count] FROM [Profile] [p] GROUP BY ([p].[Age])
```

## HAVING

Filter groups with `.Having()`. It accepts the same expressions as `.Where()`:

```csharp
SqlTable profiles = new("Profile", "p");

var sql = Sql
    .Select(
        profiles + "Age",
        SqlAggregate.Count(profiles + "Id", "Count")
    )
    .From(profiles)
    .GroupBy(profiles + "Age")
    .Having(SqlExpression.GreaterThanOrEqual(profiles + "Age", 18))
    .ToSql();
```

```sql
SELECT [p].[Age], COUNT([p].[Id]) AS [Count]
FROM [Profile] [p]
GROUP BY ([p].[Age]) HAVING [p].[Age] >= 18
```

### HAVING with aggregate functions

You can also use an aggregate function in the `HAVING` clause to filter based on the grouped result:

```csharp
var sql = Sql
    .Select("Department", SqlAggregate.Count("Id", "Headcount"))
    .From("Employees")
    .GroupBy("Department")
    .Having(SqlExpression.GreaterThan(SqlAggregate.Count("Id"), 10))
    .ToSql();
```

```sql
SELECT [Department], COUNT([Id]) AS [Headcount]
FROM [Employees]
GROUP BY ([Department]) HAVING COUNT([Id]) > 10
```

### HAVING with BETWEEN on an aggregate

```csharp
SqlTable products = new("Products", "p");

var sql = Sql
    .Select(
        products + "Category",
        SqlAggregate.Average(products + "Price", "AvgPrice")
    )
    .From(products)
    .GroupBy(products + "Category")
    .Having(SqlExpression.Between(SqlAggregate.Average(products + "Price"), 50, 200))
    .ToSql();
```

```sql
SELECT [p].[Category], AVG([p].[Price]) AS [AvgPrice]
FROM [Products] [p]
GROUP BY ([p].[Category]) HAVING AVG([p].[Price]) BETWEEN 50 AND 200
```

## Aggregate functions

All aggregate functions are available on the `SqlAggregate` static class. They return a `SqlFunction` that can be used in `SELECT`, `HAVING`, and `ORDER BY`.

| Method | SQL | Description |
|---|---|---|
| `SqlAggregate.Count(column)` | `COUNT([column])` | Number of non-null values |
| `SqlAggregate.Sum(column)` | `SUM([column])` | Sum of values |
| `SqlAggregate.Average(column)` | `AVG([column])` | Arithmetic mean |
| `SqlAggregate.Max(column)` | `MAX([column])` | Largest value |
| `SqlAggregate.Min(column)` | `MIN([column])` | Smallest value |

Each accepts an optional `alias` parameter to add an `AS` clause.

### Complete example

```csharp
var sql = Sql
    .Select(
        "Department",
        SqlAggregate.Count("Id", "Headcount"),
        SqlAggregate.Average("Salary", "AvgSalary"),
        SqlAggregate.Max("Salary", "MaxSalary"),
        SqlAggregate.Min("Salary", "MinSalary"),
        SqlAggregate.Sum("Salary", "TotalPayroll")
    )
    .From("Employees")
    .GroupBy("Department")
    .ToSql();
```

```sql
SELECT [Department],
       COUNT([Id]) AS [Headcount],
       AVG([Salary]) AS [AvgSalary],
       MAX([Salary]) AS [MaxSalary],
       MIN([Salary]) AS [MinSalary],
       SUM([Salary]) AS [TotalPayroll]
FROM [Employees]
GROUP BY ([Department])
```

### Aggregates without GROUP BY

Aggregates can also be used without `GROUP BY` to compute a single value over the entire table:

```csharp
var sql = Sql
    .Select(SqlAggregate.Count("Id", "TotalUsers"))
    .From("Users")
    .ToSql();
```

```sql
SELECT COUNT([Id]) AS [TotalUsers] FROM [Users]
```

### Aggregates with joins

```csharp
SqlTable o = new("Orders", "o");
SqlTable c = new("Customers", "c");

var sql = Sql
    .Select(
        c + "Name",
        SqlAggregate.Count(o + "Id", "OrderCount"),
        SqlAggregate.Sum(o + "Total", "TotalSpent")
    )
    .From(c)
    .LeftJoin(o).On(SqlExpression.Equal(c + "Id", o + "CustomerId"))
    .GroupBy(c + "Name")
    .Having(SqlExpression.GreaterThan(SqlAggregate.Sum(o + "Total"), 1000))
    .OrderBy(c + "Name")
    .ToSql();
```

```sql
SELECT [c].[Name], COUNT([o].[Id]) AS [OrderCount], SUM([o].[Total]) AS [TotalSpent]
FROM [Customers] [c]
LEFT JOIN [Orders] [o] ON [c].[Id] = [o].[CustomerId]
GROUP BY ([c].[Name]) HAVING SUM([o].[Total]) > 1000
ORDER BY [c].[Name] ASC
```
