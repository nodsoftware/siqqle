# Selecting Data

`SELECT` statements are built with `Sql.Select()`, followed by `.From()` and optional clauses.

## Basic select

Pass column names as strings — they are implicitly converted to `SqlColumn` instances:

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("User")
    .Where(SqlExpression.Equal("Id", 5))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [User] WHERE [Id] = 5
```

## Select all columns

```csharp
var sql = Sql.Select("*").From("User").ToSql();
```

```sql
SELECT * FROM [User]
```

## Implicit conversions

Several C# types are implicitly convertible to Siqqle expression types, which is what makes the fluent API concise:

| C# type | Converts to | Example |
|---|---|---|
| `string` | `SqlColumn` (in column position) | `"Name"` → `[Name]` |
| `string` | `SqlTable` (in table position) | `"Users"` → `[Users]` |
| `string` | `SqlConstant` (in value position) | `"Active"` → `'Active'` |
| `int`, `long`, `short` | `SqlConstant` | `42` → `42` |
| `decimal`, `float`, `double` | `SqlConstant` | `3.14` → `3.14` |
| `bool` | `SqlConstant` | `true` → `True` |
| `DateTime`, `DateTimeOffset` | `SqlConstant` | — |
| `Guid` | `SqlConstant` | — |

The compiler resolves the correct conversion based on the method signature. For example, `Sql.Select("Id")` converts `"Id"` to `SqlColumn`, while `SqlExpression.Equal("Status", "Active")` converts `"Status"` to `SqlColumn` and `"Active"` to `SqlConstant`.

When the implicit conversion is ambiguous, use an explicit cast:

```csharp
// Explicit cast to SqlColumn in a Select that accepts SqlValue[]
Sql.Select((SqlColumn)"Id", (SqlColumn)"Name").From("Users").ToSql();
```

## Column aliases

Use `SqlColumn` with an alias to rename a column in the result:

```csharp
var author = new SqlColumn("CreatedBy", "Author");

var sql = Sql
    .Select(author)
    .From("Post")
    .ToSql();
```

```sql
SELECT [CreatedBy] AS [Author] FROM [Post]
```

You can also add an alias using the `+` operator:

```csharp
SqlColumn col = (SqlColumn)"CreatedBy" + "Author";
```

## Table aliases

Use `SqlTable` with an alias when you need to reference a table by a short name — for example in joins. Columns are accessed using the `+` operator:

```csharp
SqlTable u = new("Users", "u");
SqlTable p = new("Profiles", "p");

var sql = Sql
    .Select(u + "Id", u + "UserName", p + "Email", p + "Age")
    .From(u)
    .LeftJoin(p)
    .On(SqlExpression.Equal(u + "Id", p + "UserId"))
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[UserName], [p].[Email], [p].[Age]
FROM [Users] [u]
LEFT JOIN [Profiles] [p] ON [u].[Id] = [p].[UserId]
```

## Schema-qualified table names

Pass a dotted name to reference a schema. Siqqle parses the segments and quotes each part:

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

## Scalar functions in SELECT

`SqlFunction` lets you include any SQL function call in the column list:

```csharp
var sql = Sql.Select(new SqlFunction("SCOPE_IDENTITY")).ToSql();
```

```sql
SELECT SCOPE_IDENTITY()
```

Pass arguments and an optional alias:

```csharp
var sql = Sql
    .Select(new SqlFunction("COALESCE", [(SqlColumn)"Name", (SqlColumn)"Email"], "DisplayName"))
    .From("Users")
    .ToSql();
```

```sql
SELECT COALESCE([Name], [Email]) AS [DisplayName] FROM [Users]
```

### Built-in helpers: LOWER / UPPER

`SqlFunction.Lower()` and `SqlFunction.Upper()` wrap a value in `LOWER()` / `UPPER()`:

```csharp
var sql = Sql
    .Select(SqlFunction.Lower((SqlColumn)"Email"), SqlFunction.Upper((SqlColumn)"Name"))
    .From("Users")
    .ToSql();
```

```sql
SELECT LOWER([Email]), UPPER([Name]) FROM [Users]
```

Functions can be nested:

```csharp
var sql = Sql
    .Select(SqlFunction.Upper(SqlFunction.Lower((SqlColumn)"Name")))
    .From("Users")
    .ToSql();
```

```sql
SELECT UPPER(LOWER([Name])) FROM [Users]
```

## Aggregate functions in SELECT

Use `SqlAggregate` to include `COUNT`, `SUM`, `AVG`, `MIN`, or `MAX` in the column list. Each accepts an optional alias:

```csharp
var sql = Sql
    .Select(
        "Department",
        SqlAggregate.Count("Id", "Headcount"),
        SqlAggregate.Sum("Salary", "TotalPayroll"),
        SqlAggregate.Average("Salary", "AvgSalary")
    )
    .From("Employees")
    .GroupBy("Department")
    .ToSql();
```

```sql
SELECT [Department],
       COUNT([Id]) AS [Headcount],
       SUM([Salary]) AS [TotalPayroll],
       AVG([Salary]) AS [AvgSalary]
FROM [Employees]
GROUP BY ([Department])
```

See [Grouping & Aggregates](grouping.md) for the complete reference.

## Arithmetic expressions in SELECT

Use `SqlExpression.Add()`, `Subtract()`, `Multiply()`, `Divide()`, and `Modulo()` to build computed columns:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Name",
        SqlExpression.Multiply((SqlColumn)"Quantity", (SqlColumn)"UnitPrice")
    )
    .From("OrderItems")
    .ToSql();
```

```sql
SELECT [Name], [Quantity] * [UnitPrice] FROM [OrderItems]
```

See [Expressions — Arithmetic](expressions.md#arithmetic-expressions) for all operators.

## DISTINCT

Call `.Distinct()` immediately after `.Select()` to eliminate duplicate rows:

```csharp
var sql = Sql.Select("City", "Country").Distinct().From("Users").ToSql();
```

```sql
SELECT DISTINCT [City], [Country] FROM [Users]
```

## CAST in SELECT

Convert a column to a different data type:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Name",
        SqlExpression.Cast((SqlColumn)"Age", SqlDataType.Float(10))
    )
    .From("Users")
    .ToSql();
```

```sql
SELECT [Name], CAST([Age] AS FLOAT(10)) FROM [Users]
```

See [Expressions — CAST](expressions.md#cast) for the full list of data types.

## Selecting from a subquery

Use `SqlSubquery` as the `FROM` source. A subquery used as a table **must** have an alias, created with the `+` operator:

```csharp
var inner = Sql.Select("Id", "Name").From("Users").Go() + "u";

var sql = Sql
    .Select("u.Id", "u.Name")
    .From(inner)
    .ToSql();
```

```sql
SELECT [u].[Id], [u].[Name] FROM (SELECT [Id], [Name] FROM [Users]) [u]
```

## The `.Go()` method

The fluent builder returns an intermediate `ISqlSyntaxEnd<T>` object. You can call `.ToSql()` directly on it, or call `.Go()` to materialise the underlying `SqlSelect` object when you need to pass it elsewhere (e.g. to `Sql.Union()` or to create a `SqlSubquery`):

```csharp
SqlSelect select = Sql.Select("Id").From("Users").Go();
```

!!! tip
    You do not need `.Go()` for common operations. The `Sql.Union()` method also accepts `ISqlSyntaxEnd<SqlSelect>` parameters directly.
