# SQL Dialects

A SQL dialect controls how identifiers are quoted, how parameters are prefixed, and how dialect-specific syntax (paging, stored procedures, string concatenation) is rendered.

## Available dialects

| Class | Package | Identifier quoting | Parameter prefix |
|---|---|---|---|
| `SqlDialect` (default) | `Siqqle` | `[Name]` | `@Name` |
| `SqlServerDialect` | `Siqqle.Dialects.SqlServer` | `[Name]` | `@Name` |
| `PostgreSqlDialect` | `Siqqle.Dialects.PostgreSql` | `"Name"` | `:Name` |
| `MySqlDialect` | `Siqqle.Dialects.MySql` | `` `Name` `` | `?Name` |

The **default** dialect produces SQL that is broadly compatible with SQL Server. `SqlServerDialect` is a subclass that adds T-SQL-specific behaviours such as the `EXEC` stored procedure syntax and the `+` concatenation operator.

## Passing a dialect to ToSql()

The recommended approach is to pass the dialect explicitly on each call. This is safe in any concurrent environment:

```csharp
var dialect = new PostgreSqlDialect();

var sql = Sql
    .Select("Id", "Name")
    .From("Users")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)7))
    .ToSql(dialect);
```

=== "SQL Server"

    ```sql
    SELECT [Id], [Name] FROM [Users] WHERE [Id] = @UserId
    ```

=== "PostgreSQL"

    ```sql
    SELECT "Id", "Name" FROM "Users" WHERE "Id" = :UserId
    ```

=== "MySQL"

    ```sql
    SELECT `Id`, `Name` FROM `Users` WHERE `Id` = ?UserId
    ```

## Dialect differences summary

### Paging

Each dialect renders `LIMIT`/`OFFSET` paging differently:

| Dialect | Output |
|---|---|
| Default / SQL Server | `OFFSET n ROWS FETCH FIRST n ROWS ONLY` |
| PostgreSQL | `LIMIT n OFFSET n` |
| MySQL | `LIMIT offset, count` |

See [Ordering and Paging](ordering-paging.md) for full examples.

### String concatenation

| Dialect | Syntax | Example |
|---|---|---|
| Default / PostgreSQL | <code>&#124;&#124;</code> | <code>[A] &#124;&#124; [B]</code> |
| SQL Server | `+` | `[A] + [B]` |
| MySQL | `CONCAT()` | `` CONCAT(`A`, `B`) `` |

See [Expressions — String concatenation](expressions.md#string-concatenation) for details.

### Stored procedures

=== "SQL Server"

    ```csharp
    var sql = Sql.Call("sp_GetData", "Id" + (SqlConstant)1).ToSql(new SqlServerDialect());
    ```

    ```sql
    EXEC sp_GetData @Id
    ```

=== "PostgreSQL"

    ```csharp
    var sql = Sql.Call("get_data", "id" + (SqlConstant)1).ToSql(new PostgreSqlDialect());
    ```

    ```sql
    CALL get_data(:id)
    ```

## PostgreSQL: parameter type casts

When querying PostgreSQL with typed columns (JSON, UUID, etc.), use `SqlParameter<T>` with the appropriate `NpgsqlDbType`. The `PostgreSqlDialect` appends the required `::type` cast automatically:

```csharp
// DbType.Guid  →  ::uuid
var guidParam = new SqlParameter("UserId", DbType.Guid, Guid.NewGuid());

// DbType.Xml  →  ::xml
var xmlParam = new SqlParameter("Payload", DbType.Xml, "<root/>");

// NpgsqlDbType.Jsonb  →  ::jsonb
var jsonbParam = new SqlParameter<NpgsqlDbType>("Data", NpgsqlDbType.Jsonb, "{}");
```

## Writer settings

`SqlWriterSettings` lets you customise the rendering output:

| Setting | Default | Effect |
|---|---|---|
| `WriteKeywordsInLowerCase` | `false` | `SELECT` → `select`, `WHERE` → `where` |
| `WriteDataTypesInLowerCase` | `false` | `INT` → `int`, `VARCHAR` → `varchar` |
| `WriteAscendingSortOrder` | `false` | Suppresses `ASC` after ascending `ORDER BY` |

These settings are passed through the `SqlWriter` and are primarily useful for advanced rendering scenarios or style preferences.

## Setting a process-wide dialect

!!! warning
    Setting a global dialect is **not thread-safe** in concurrent applications. Prefer passing the dialect to `ToSql()` overloads or inject it via your DI container instead.

```csharp
// Application startup only — single-threaded context
#pragma warning disable CS0618
SqlDialect.UseDialect(new PostgreSqlDialect());
#pragma warning restore CS0618
```

From that point on, calls to `ToSql()` without an explicit dialect will use the configured one.

## Dependency injection

Dialect classes are stateless and safe to share across threads, so registering them as singletons is the correct lifetime.

### Registration

```csharp
// Program.cs / Startup.cs
services.AddSingleton<PostgreSqlDialect>();
```

If you want to inject by the base type as well:

```csharp
services.AddSingleton<SqlDialect, PostgreSqlDialect>();
services.AddSingleton<PostgreSqlDialect>(sp => (PostgreSqlDialect)sp.GetRequiredService<SqlDialect>());
```

### Consuming the dialect

Inject the dialect into your class and pass it to `ToSql()`:

```csharp
class UserRepository(PostgreSqlDialect dialect)
{
    public string GetAll() =>
        Sql.Select("Id", "Name")
           .From("Users")
           .ToSql(dialect);

    public string GetById(int id) =>
        Sql.Select("Id", "Name")
           .From("Users")
           .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)id))
           .ToSql(dialect);
}
```

If you registered the dialect as `SqlDialect`, inject that instead:

```csharp
class UserRepository(SqlDialect dialect) { … }
```

## Custom dialects

Extend `SqlDialect` to support an additional database engine. Override the methods you need:

```csharp
public class MyDialect : SqlDialect
{
    public override string FormatIdentifier(string identifier) => $"`{identifier}`";
    public override string FormatParameterName(string parameterName) => $"${parameterName}";
}
```

### Overridable methods

| Method | Purpose | Default behaviour |
|---|---|---|
| `FormatIdentifier(string)` | Quote an identifier | `[name]` |
| `FormatParameterName(string)` | Prefix a parameter name | `@name` |
| `FormatString(string)` | Escape a string literal | `'escaped'` |
| `WriteCall(writer, visitor, name, args)` | Stored procedure syntax | — |
| `WriteLimit(writer, offset, count)` | Paging syntax | `OFFSET…FETCH FIRST` |
| `WriteConcatenation(writer, visitor, left, right)` | String concatenation | <code>&#124;&#124;</code> |
| `WriteParameter(writer, param)` | Parameter rendering | `@name` |
| `WriteArithmeticOperator(writer, op)` | Arithmetic operator rendering | Standard operators |
