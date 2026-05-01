# Siqqle

Siqqle is a lightweight, fluent SQL query builder for .NET that makes constructing SQL statements safe, expressive, and type-safe.

## Features

- **Fluent API** - Write SQL queries with an intuitive, chainable interface
- **Type-Safe** - Catch errors at compile-time, not at runtime
- **Multi-Dialect Support** - Build queries for SQL Server, PostgreSQL, MySQL, and more
- **Dapper Integration** - Seamlessly execute queries with Dapper
- **No ORM** - Direct control over your SQL, without the overhead of an ORM

## Installation

Available via NuGet:

```bash
dotnet add package Siqqle
```

For Dapper integration:

```bash
dotnet add package Siqqle.Dapper
```

## Quick Start

```csharp
using Siqqle;

// Create a SELECT query
var query = Sql.Select("Name", "Email")
    .From("Users")
    .Where("IsActive = 1")
    .OrderBy("Name");

string sql = query.ToSql();
// SELECT Name, Email FROM Users WHERE IsActive = 1 ORDER BY Name
```

## Documentation

For detailed documentation, visit the [documentation site](https://nodsoftware.github.io/siqqle/).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
