# Deleting Data

`DELETE` statements are built with `Sql.Delete()`, followed by `.From()` and an optional `.Where()`.

## Basic delete

```csharp
var sql = Sql
    .Delete()
    .From("Users")
    .Where(SqlExpression.Equal("Id", 42))
    .ToSql();
```

```sql
DELETE FROM [Users] WHERE [Id] = 42
```

## Delete with compound condition

Combine multiple conditions with `SqlExpression.And()` or `SqlExpression.Or()`:

```csharp
var sql = Sql
    .Delete()
    .From("AuditLog")
    .Where(
        SqlExpression.And(
            SqlExpression.LessThan("CreatedAt", "CutOff" + (SqlConstant)DateTime.UtcNow),
            SqlExpression.Equal("Archived", true)
        )
    )
    .ToSql();
```

```sql
DELETE FROM [AuditLog] WHERE ([CreatedAt] < @CutOff) AND ([Archived] = True)
```

## Delete with parameters

```csharp
var sql = Sql
    .Delete()
    .From("Sessions")
    .Where(SqlExpression.Equal("UserId", "UserId" + (SqlConstant)7))
    .ToSql();
```

```sql
DELETE FROM [Sessions] WHERE [UserId] = @UserId
```

## Delete with IN

```csharp
var sql = Sql
    .Delete()
    .From("Notifications")
    .Where(SqlExpression.In("Status", "Read", "Dismissed"))
    .ToSql();
```

```sql
DELETE FROM [Notifications] WHERE [Status] IN ('Read', 'Dismissed')
```

## Delete with IN subquery

Use a subquery to identify the rows to delete:

```csharp
var subquery = new SqlSubquery(
    Sql.Select("UserId")
       .From("BlockedUsers")
       .Where(SqlExpression.Equal("Reason", "Spam"))
);

var sql = Sql
    .Delete()
    .From("Comments")
    .Where(SqlExpression.In("AuthorId", subquery))
    .ToSql();
```

```sql
DELETE FROM [Comments] WHERE [AuthorId] IN (SELECT [UserId] FROM [BlockedUsers] WHERE [Reason] = 'Spam')
```

## Delete with EXISTS

Use a correlated subquery with `EXISTS` to delete rows that have matching records in another table:

```csharp
var subquery = new SqlSubquery(
    Sql.Select(1)
       .From("ExpiredTokens")
       .Where(SqlExpression.Equal(
           new SqlColumn("ExpiredTokens.UserId"),
           new SqlColumn("Sessions.UserId")
       ))
);

var sql = Sql
    .Delete()
    .From("Sessions")
    .Where(SqlExpression.Exists(subquery))
    .ToSql();
```

```sql
DELETE FROM [Sessions]
WHERE EXISTS (SELECT 1 FROM [ExpiredTokens] WHERE [ExpiredTokens].[UserId] = [Sessions].[UserId])
```

## Delete all rows

Omitting `.Where()` deletes **all rows** in the table:

```csharp
var sql = Sql
    .Delete()
    .From("TempImport")
    .ToSql();
```

```sql
DELETE FROM [TempImport]
```

!!! warning
    A `DELETE` without a `WHERE` clause removes every row from the table. Ensure this is intentional before executing.

## Delete with Dapper

With `Siqqle.Dapper`, the `Execute` method returns the number of rows affected:

```csharp
int deleted = connection.Execute(
    Sql.Delete()
       .From("Sessions")
       .Where(SqlExpression.Equal("UserId", "UserId" + (SqlConstant)7))
);
```

See [Dapper Integration](dapper.md) for the full API.
