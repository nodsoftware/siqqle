# Installation

Siqqle is distributed as a set of NuGet packages. Install only what you need.

## Prerequisites

Siqqle targets **.NET 8.0** and **.NET 10.0**. Make sure your project targets one of these frameworks (or higher).

## Core package

The `Siqqle` package contains the query builder, expression types, and the default SQL dialect. It is required by all other Siqqle packages.

=== ".NET CLI"

    ```
    dotnet add package Siqqle
    ```

=== "Package Manager"

    ```
    Install-Package Siqqle
    ```

=== "PackageReference"

    ```xml
    <PackageReference Include="Siqqle" Version="6.*" />
    ```

## Dialect packages

Install a dialect package when you need output tailored for a specific database engine. Each dialect controls identifier quoting, parameter prefixes, paging syntax, and other engine-specific behaviour.

| Package | Database | Identifier quoting | Parameter prefix |
|---|---|---|---|
| `Siqqle.Dialects.SqlServer` | SQL Server (T-SQL) | `[Name]` | `@Name` |
| `Siqqle.Dialects.PostgreSql` | PostgreSQL | `"Name"` | `:Name` |
| `Siqqle.Dialects.MySql` | MySQL / MariaDB | `` `Name` `` | `?Name` |

=== ".NET CLI"

    ```
    dotnet add package Siqqle.Dialects.SqlServer
    dotnet add package Siqqle.Dialects.PostgreSql
    dotnet add package Siqqle.Dialects.MySql
    ```

=== "PackageReference"

    ```xml
    <PackageReference Include="Siqqle.Dialects.SqlServer" Version="6.*" />
    <PackageReference Include="Siqqle.Dialects.PostgreSql" Version="6.*" />
    <PackageReference Include="Siqqle.Dialects.MySql" Version="6.*" />
    ```

!!! tip
    The core `Siqqle` package already includes a default dialect that produces SQL with `[square bracket]` quoting and `@` parameter prefixes — identical to SQL Server conventions. You only need `Siqqle.Dialects.SqlServer` if you require T-SQL-specific features such as the `EXEC` stored procedure syntax or the `+` concatenation operator.

## Dapper integration

The `Siqqle.Dapper` package adds extension methods to `IDbConnection` that accept Siqqle statements directly, with automatic parameter extraction.

=== ".NET CLI"

    ```
    dotnet add package Siqqle.Dapper
    ```

=== "PackageReference"

    ```xml
    <PackageReference Include="Siqqle.Dapper" Version="6.*" />
    ```

## Dependency injection

The `Siqqle.Extensions.DependencyInjection` package provides `IServiceCollection` extensions to register a SQL dialect and an injectable `SqlBuilder<TDialect>` in your DI container.

=== ".NET CLI"

    ```
    dotnet add package Siqqle.Extensions.DependencyInjection
    ```

=== "PackageReference"

    ```xml
    <PackageReference Include="Siqqle.Extensions.DependencyInjection" Version="6.*" />
    ```

See [SQL Dialects — Dependency injection](dialects.md#dependency-injection) for usage.

## Package summary

| Package | Purpose |
|---|---|
| `Siqqle` | Core query builder and default dialect |
| `Siqqle.Dialects.SqlServer` | SQL Server / T-SQL dialect |
| `Siqqle.Dialects.PostgreSql` | PostgreSQL dialect |
| `Siqqle.Dialects.MySql` | MySQL / MariaDB dialect |
| `Siqqle.Dapper` | Dapper extension methods for Siqqle statements |
| `Siqqle.Extensions.DependencyInjection` | ASP.NET Core DI integration |

## Namespaces

Add these `using` directives at the top of your files:

```csharp
using Siqqle;                // Sql static class, ToSql extensions
using Siqqle.Expressions;    // SqlExpression, SqlColumn, SqlTable, SqlAggregate, ...
```

When using a dialect, also add the relevant namespace:

```csharp
using Siqqle.Dialects.SqlServer;
using Siqqle.Dialects.PostgreSql;
using Siqqle.Dialects.MySql;
```

When using Dapper integration:

```csharp
using Siqqle.Dapper;
```

When using dependency injection:

```csharp
using Siqqle.Extensions.DependencyInjection;
```
