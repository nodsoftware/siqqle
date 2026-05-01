# Expressions

This page covers the more advanced expression types: arithmetic, string concatenation, `CASE`, `CAST`, scalar functions, stored procedure calls, and batches.

## Arithmetic expressions

Siqqle supports all standard arithmetic operators via `SqlExpression` factory methods:

| Method | SQL operator |
|---|---|
| `SqlExpression.Add(left, right)` | `+` |
| `SqlExpression.Subtract(left, right)` | `-` |
| `SqlExpression.Multiply(left, right)` | `*` |
| `SqlExpression.Divide(left, right)` | `/` |
| `SqlExpression.Modulo(left, right)` | `%` |

Both operands can be columns, constants, parameters, or other expressions.

### Computed column

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

### Arithmetic with constants

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Name",
        SqlExpression.Add((SqlColumn)"Price", (SqlConstant)10)
    )
    .From("Products")
    .ToSql();
```

```sql
SELECT [Name], [Price] + 10 FROM [Products]
```

### Nested arithmetic

Chain operations to build complex formulas:

```csharp
// (Price * Quantity) - Discount
var total = SqlExpression.Subtract(
    SqlExpression.Multiply((SqlColumn)"Price", (SqlColumn)"Quantity"),
    (SqlColumn)"Discount"
);

var sql = Sql
    .Select((SqlColumn)"OrderId", total)
    .From("OrderLines")
    .ToSql();
```

```sql
SELECT [OrderId], ([Price] * [Quantity]) - [Discount] FROM [OrderLines]
```

### Arithmetic in WHERE

```csharp
var sql = Sql
    .Select("Id", "Name")
    .From("Products")
    .Where(SqlExpression.GreaterThan(
        SqlExpression.Multiply((SqlColumn)"Price", (SqlColumn)"Quantity"),
        (SqlConstant)1000
    ))
    .ToSql();
```

```sql
SELECT [Id], [Name] FROM [Products] WHERE ([Price] * [Quantity]) > 1000
```

### Arithmetic in UPDATE

Increment a column value:

```csharp
var sql = Sql
    .Update("Products")
    .Set("Stock", SqlExpression.Subtract((SqlColumn)"Stock", (SqlConstant)1))
    .Where(SqlExpression.Equal("Id", 5))
    .ToSql();
```

```sql
UPDATE [Products] SET [Stock] = [Stock] - 1 WHERE [Id] = 5
```

## String concatenation

Use `SqlExpression.Concat()` to concatenate two values. The SQL output depends on the dialect:

| Dialect | Concatenation syntax |
|---|---|
| Default / PostgreSQL | `left \|\| right` |
| SQL Server | `left + right` |
| MySQL | `CONCAT(left, right)` |

```csharp
var sql = Sql
    .Select(SqlExpression.Concat((SqlColumn)"FirstName", (SqlColumn)"LastName"))
    .From("Users")
    .ToSql();
```

=== "Default / PostgreSQL"

    ```sql
    SELECT [FirstName] || [LastName] FROM [Users]
    ```

=== "SQL Server"

    ```sql
    SELECT [FirstName] + [LastName] FROM [Users]
    ```

=== "MySQL"

    ```sql
    SELECT CONCAT(`FirstName`, `LastName`) FROM `Users`
    ```

Chain `Concat` calls to join multiple values:

```csharp
var fullName = SqlExpression.Concat(
    SqlExpression.Concat((SqlColumn)"FirstName", (SqlConstant)" "),
    (SqlColumn)"LastName"
);

var sql = Sql.Select(fullName).From("Users").ToSql();
```

```sql
SELECT ([FirstName] || ' ') || [LastName] FROM [Users]
```

## CASE WHEN

Use `SqlExpression.Case()` to build a searched `CASE` expression. Chain `.When().Then()` pairs and close with `.Else().End(alias)`:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Id",
        (SqlColumn)"Name",
        SqlExpression
            .Case()
            .When(SqlExpression.GreaterThan("Age", 60)).Then("Elder")
            .When(SqlExpression.GreaterThan("Age", 18)).Then("Adult")
            .Else("Child")
            .End("AgeGroup")
    )
    .From("User")
    .ToSql();
```

```sql
SELECT [Id], [Name],
       CASE WHEN [Age] > 60 THEN 'Elder'
            WHEN [Age] > 18 THEN 'Adult'
            ELSE 'Child'
       END AS [AgeGroup]
FROM [User]
```

### CASE without ELSE

The `.Else()` clause is optional — omit it and the CASE returns `NULL` when no condition matches:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Id",
        SqlExpression
            .Case()
            .When(SqlExpression.Equal("Status", "Active")).Then("Yes")
            .End("IsActive")
    )
    .From("Users")
    .ToSql();
```

```sql
SELECT [Id],
       CASE WHEN [Status] = 'Active' THEN 'Yes'
       END AS [IsActive]
FROM [Users]
```

### CASE without alias

If you don't need an alias, call `.End()` with no arguments:

```csharp
SqlExpression
    .Case()
    .When(SqlExpression.GreaterThan("Score", 90)).Then("A")
    .When(SqlExpression.GreaterThan("Score", 80)).Then("B")
    .Else("C")
    .End()
```

## CASE value WHEN

The `Case(value)` overload generates a simple (value-based) `CASE`:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Id",
        (SqlColumn)"Name",
        SqlExpression
            .Case((SqlColumn)"Age")
            .When(30).Then("Thirty")
            .When(20).Then("Twenty")
            .Else((SqlColumn)"Age")
            .End("AgeString")
    )
    .From("User")
    .ToSql();
```

```sql
SELECT [Id], [Name],
       CASE [Age] WHEN 30 THEN 'Thirty'
                  WHEN 20 THEN 'Twenty'
                  ELSE [Age]
       END AS [AgeString]
FROM [User]
```

## CAST

Convert a value to a different data type with `SqlExpression.Cast()`:

```csharp
var sql = Sql
    .Select(
        (SqlColumn)"Id",
        (SqlColumn)"Name",
        SqlExpression.Cast((SqlColumn)"Age", SqlDataType.Float(10))
    )
    .From("User")
    .ToSql();
```

```sql
SELECT [Id], [Name], CAST([Age] AS FLOAT(10)) FROM [User]
```

### Available data types

`SqlDataType` exposes static factory methods for every common SQL type:

| Method | SQL type |
|---|---|
| `SqlDataType.Bit()` | `BIT` |
| `SqlDataType.TinyInt()` | `TINYINT` |
| `SqlDataType.SmallInt()` | `SMALLINT` |
| `SqlDataType.Int()` | `INT` |
| `SqlDataType.BigInt()` | `BIGINT` |
| `SqlDataType.Float(precision)` | `FLOAT(n)` |
| `SqlDataType.Real()` | `REAL` |
| `SqlDataType.Decimal(precision, scale)` | `DECIMAL(p, s)` |
| `SqlDataType.Char(size)` | `CHAR(n)` |
| `SqlDataType.VarChar(size)` | `VARCHAR(n)` |
| `SqlDataType.NChar(size)` | `NCHAR(n)` |
| `SqlDataType.NVarChar(size)` | `NVARCHAR(n)` |
| `SqlDataType.Binary(size)` | `BINARY(n)` |
| `SqlDataType.VarBinary(size)` | `VARBINARY(n)` |
| `SqlDataType.Date()` | `DATE` |
| `SqlDataType.Time()` | `TIME` |
| `SqlDataType.DateTime()` | `DATETIME` |
| `SqlDataType.DateTime2()` | `DATETIME2` |
| `SqlDataType.UniqueIdentifier()` | `UNIQUEIDENTIFIER` |

### CAST examples

```csharp
// Cast to VARCHAR(100)
SqlExpression.Cast((SqlColumn)"Name", SqlDataType.VarChar(100))

// Cast to DECIMAL(10, 2)
SqlExpression.Cast((SqlColumn)"Price", SqlDataType.Decimal(10, 2))

// Cast to INT
SqlExpression.Cast((SqlColumn)"StringValue", SqlDataType.Int())
```

## Scalar functions

Use `SqlFunction` to call any SQL function by name:

```csharp
// Arbitrary function
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

```csharp
var sql = Sql
    .Select(
        SqlFunction.Lower((SqlColumn)"Email"),
        SqlFunction.Upper((SqlColumn)"Name")
    )
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

### Custom function with multiple arguments

```csharp
var sql = Sql
    .Select(new SqlFunction("ISNULL", [(SqlColumn)"MiddleName", (SqlConstant)"N/A"], "MiddleName"))
    .From("Users")
    .ToSql();
```

```sql
SELECT ISNULL([MiddleName], 'N/A') AS [MiddleName] FROM [Users]
```

## Stored procedures

Use `Sql.Call()` to generate a stored procedure invocation. The SQL produced depends on the dialect.

```csharp
var sql = Sql
    .Call("sp_GetUserById", "UserId" + (SqlConstant)42)
    .ToSql();
```

=== "SQL Server"

    ```sql
    EXEC sp_GetUserById @UserId
    ```

=== "PostgreSQL"

    ```sql
    CALL sp_GetUserById(:UserId)
    ```

### Stored procedure with multiple arguments

```csharp
var sql = Sql
    .Call(
        "sp_SearchUsers",
        "Name" + (SqlConstant)"John",
        "MinAge" + (SqlConstant)18,
        "MaxAge" + (SqlConstant)65
    )
    .ToSql(new SqlServerDialect());
```

```sql
EXEC sp_SearchUsers @Name, @MinAge, @MaxAge
```

## Batching statements

`Sql.Batch()` concatenates multiple statements, separating them with a statement delimiter (`;`):

```csharp
var sql = Sql.Batch(
    Sql.Insert().Into("Log", "Message").Values("Started"),
    Sql.Update("Job").Set("Status", "Running").Where(SqlExpression.Equal("Id", 1)),
    Sql.Insert().Into("Log", "Message").Values("Done")
).ToSql();
```

```sql
INSERT INTO [Log] ([Message]) VALUES ('Started');
UPDATE [Job] SET [Status] = 'Running' WHERE [Id] = 1;
INSERT INTO [Log] ([Message]) VALUES ('Done')
```

Batches accept any `SqlStatement` types — you can mix `INSERT`, `UPDATE`, `DELETE`, and `SELECT` statements:

```csharp
var sql = Sql.Batch(
    Sql.Delete().From("TempData"),
    Sql.Insert().Into("TempData", "Value").Values("Refreshed"),
    Sql.Select("Value").From("TempData")
).ToSql();
```

```sql
DELETE FROM [TempData];
INSERT INTO [TempData] ([Value]) VALUES ('Refreshed');
SELECT [Value] FROM [TempData]
```

`Sql.Batch()` also accepts `ISqlSyntaxEnd<SqlStatement>` parameters, so you can pass fluent builder results directly without calling `.Go()`.

### Batches with Dapper

Use `QueryMultiple` to process multiple result sets from a batch:

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

See [Dapper Integration](dapper.md) for more details.
```
