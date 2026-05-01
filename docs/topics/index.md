# Introduction

Siqqle is a .NET library for building SQL statements in code. It provides a fluent API that lets you construct type-safe SQL expression trees and render them as SQL text for use with ADO.NET, Dapper, or any other data access layer.

## Why Siqqle?

Hard-coded SQL strings scatter query logic throughout your codebase, are difficult to compose dynamically, and offer no help from the compiler. Siqqle solves this by representing queries as structured expression trees that you build in C# and render to SQL when needed.

```csharp
var sql = Sql
    .Select("Id", "UserName", "Email")
    .From("Users")
    .Where(SqlExpression.GreaterThanOrEqual("Age", 18))
    .OrderBy("UserName")
    .ToSql();
```

```sql
SELECT [Id], [UserName], [Email] FROM [Users] WHERE [Age] >= 18 ORDER BY [UserName] ASC
```

### Benefits over raw SQL strings

- **Composability** — build queries from reusable fragments, conditionally add clauses, and combine statements with `UNION` or `Batch`.
- **Compile-time safety** — the fluent API guides you through valid clause order; the compiler catches mistakes before your code runs.
- **Dialect portability** — write one query and render it for SQL Server, PostgreSQL, or MySQL by swapping the dialect.
- **Parameter safety** — named parameters are first-class citizens, so values are never embedded in the SQL text.

## Features

- Fluent, strongly-typed query builder with compile-time clause validation
- Full support for `SELECT`, `INSERT`, `UPDATE`, and `DELETE`
- `JOIN` (`INNER`, `LEFT`, `RIGHT`), `GROUP BY`, `HAVING`, `ORDER BY`, `LIMIT`/`OFFSET`, `DISTINCT`
- Subqueries, `UNION`, `EXISTS`, `IN`, `BETWEEN`, `LIKE`, `IS NULL`
- `CASE`/`WHEN`, `CAST`, aggregate and scalar functions (`COUNT`, `SUM`, `AVG`, `MIN`, `MAX`)
- Arithmetic expressions (`+`, `-`, `*`, `/`, `%`) and string concatenation
- Named SQL parameters with `DbType` support and callback-based extraction
- Multiple SQL dialects: default, SQL Server, PostgreSQL, MySQL — with custom dialect extensibility
- First-class [Dapper](dapper.md) integration via `Siqqle.Dapper`
- ASP.NET Core [dependency injection](dialects.md#dependency-injection) via `Siqqle.Extensions.DependencyInjection`
- Stored procedure calls and statement batching
- Targets .NET 8 and .NET 10

## Requirements

- .NET 8.0 or .NET 10.0

## Getting started

Head to the [Installation](installation.md) page to add the NuGet packages, then follow the [Quick Start](quick-start.md) guide to write your first queries.

## License

Siqqle is open-source software released under the [MIT license](https://github.com/WouterDemuynck/siqqle/blob/master/LICENSE.md).
