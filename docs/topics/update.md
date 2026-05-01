# Updating Data

`UPDATE` statements are built with `Sql.Update(table)`, followed by one or more `.Set()` calls and an optional `.Where()`.

## Basic update

```csharp
var sql = Sql
    .Update("Users")
    .Set("UserName", "jdoe_updated")
    .Where(SqlExpression.Equal("Id", 4))
    .ToSql();
```

```sql
UPDATE [Users] SET [UserName] = 'jdoe_updated' WHERE [Id] = 4
```

## Updating multiple columns

Chain `.Set()` for each column you want to change:

```csharp
var sql = Sql
    .Update("Users")
    .Set("UserName", "jdoe_new")
    .Set("Email", "new@example.com")
    .Set("UpdatedAt", SqlConstant.Null)
    .Where(SqlExpression.Equal("Id", 42))
    .ToSql();
```

```sql
UPDATE [Users] SET [UserName] = 'jdoe_new', [Email] = 'new@example.com', [UpdatedAt] = NULL WHERE [Id] = 42
```

## Update with NULL

Pass `null` (or `SqlConstant.Null`) to set a column to `NULL`:

```csharp
var sql = Sql
    .Update("Users")
    .Set("DeletedAt", null)
    .Where(SqlExpression.Equal("Id", 7))
    .ToSql();
```

```sql
UPDATE [Users] SET [DeletedAt] = NULL WHERE [Id] = 7
```

## Update with parameters

```csharp
var sql = Sql
    .Update("Users")
    .Set("Email", "Email" + (SqlConstant)"new@example.com")
    .Where(SqlExpression.Equal("Id", "UserId" + (SqlConstant)42))
    .ToSql();
```

```sql
UPDATE [Users] SET [Email] = @Email WHERE [Id] = @UserId
```

## Update with arithmetic expression

Use arithmetic expressions to compute the new value from existing columns — for example, incrementing a counter:

```csharp
var sql = Sql
    .Update("Products")
    .Set("Stock", SqlExpression.Subtract((SqlColumn)"Stock", (SqlConstant)1))
    .Where(SqlExpression.Equal("Id", "ProductId" + (SqlConstant)5))
    .ToSql();
```

```sql
UPDATE [Products] SET [Stock] = [Stock] - 1 WHERE [Id] = @ProductId
```

Applying a percentage increase:

```csharp
var sql = Sql
    .Update("Products")
    .Set("Price", SqlExpression.Multiply((SqlColumn)"Price", (SqlConstant)1.10m))
    .Where(SqlExpression.Equal("Category", "Electronics"))
    .ToSql();
```

```sql
UPDATE [Products] SET [Price] = [Price] * 1.10 WHERE [Category] = 'Electronics'
```

## Update using a table alias

Pass a `SqlTable` with an alias to `Sql.Update()`:

```csharp
SqlTable u = new("Users", "u");

var sql = Sql
    .Update(u)
    .Set("Email", "Email" + (SqlConstant)"updated@example.com")
    .Where(SqlExpression.Equal(u + "Id", "UserId" + (SqlConstant)5))
    .ToSql();
```

```sql
UPDATE [Users] [u] SET [Email] = @Email WHERE [u].[Id] = @UserId
```

## Update with compound WHERE

```csharp
var sql = Sql
    .Update("Users")
    .Set("IsActive", false)
    .Where(
        SqlExpression.And(
            SqlExpression.LessThan("LastLoginAt", "CutOff" + (SqlConstant)DateTime.UtcNow),
            SqlExpression.Equal("IsActive", true)
        )
    )
    .ToSql();
```

```sql
UPDATE [Users] SET [IsActive] = False WHERE ([LastLoginAt] < @CutOff) AND ([IsActive] = True)
```

## Update without WHERE

Omitting `.Where()` updates **all rows** in the table. Make sure that is the intent:

```csharp
var sql = Sql
    .Update("Settings")
    .Set("IsActive", true)
    .ToSql();
```

```sql
UPDATE [Settings] SET [IsActive] = True
```

!!! warning
    An `UPDATE` without a `WHERE` clause modifies every row in the table. Ensure this is intentional before executing.
