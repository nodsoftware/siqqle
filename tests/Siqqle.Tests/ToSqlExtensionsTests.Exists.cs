using Xunit;

namespace Siqqle.Expressions.Tests;

public partial class ToSqlExtensionsTests
{
    [Fact]
    public void ToSql_WithExists_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("Orders")
                .Where(
                    SqlExpression.Equal(new SqlColumn("Orders.UserId"), new SqlColumn("Users.Id"))
                )
        );

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.Exists(subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotExists_ReturnsSqlWithNotExists()
    {
        const string expected =
            "SELECT [Id], [Name] FROM [Users] WHERE NOT EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[Id])";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("Orders")
                .Where(
                    SqlExpression.Equal(new SqlColumn("Orders.UserId"), new SqlColumn("Users.Id"))
                )
        );

        var actual = Sql.Select("Id", "Name")
            .From("Users")
            .Where(SqlExpression.NotExists(subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsMultipleConditions_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [ProductId] FROM [Products] WHERE ([Price] > 100) AND (EXISTS (SELECT 1 FROM [OrderItems] WHERE [OrderItems].[ProductId] = [Products].[ProductId]))";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("OrderItems")
                .Where(
                    SqlExpression.Equal(
                        new SqlColumn("OrderItems.ProductId"),
                        new SqlColumn("Products.ProductId")
                    )
                )
        );

        var actual = Sql.Select("ProductId")
            .From("Products")
            .Where(
                SqlExpression.And(
                    SqlExpression.GreaterThan("Price", 100),
                    SqlExpression.Exists(subquery)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNotExistsMultipleConditions_ReturnsSqlWithNotExists()
    {
        const string expected =
            "SELECT [UserId] FROM [Users] WHERE ([IsActive] = 1) AND (NOT EXISTS (SELECT 1 FROM [Logins] WHERE [Logins].[UserId] = [Users].[UserId]))";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("Logins")
                .Where(
                    SqlExpression.Equal(
                        new SqlColumn("Logins.UserId"),
                        new SqlColumn("Users.UserId")
                    )
                )
        );

        var actual = Sql.Select("UserId")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.Equal("IsActive", 1),
                    SqlExpression.NotExists(subquery)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsOrCondition_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [Id] FROM [Customers] WHERE ([Status] = 'VIP') OR (EXISTS (SELECT 1 FROM [Orders] WHERE ([Orders].[CustomerId] = [Customers].[Id]) AND ([Orders].[Total] > 1000)))";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("Orders")
                .Where(
                    SqlExpression.And(
                        SqlExpression.Equal(
                            new SqlColumn("Orders.CustomerId"),
                            new SqlColumn("Customers.Id")
                        ),
                        SqlExpression.GreaterThan(new SqlColumn("Orders.Total"), 1000)
                    )
                )
        );

        var actual = Sql.Select("Id")
            .From("Customers")
            .Where(
                SqlExpression.Or(
                    SqlExpression.Equal("Status", "VIP"),
                    SqlExpression.Exists(subquery)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsComplexSubquery_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [DepartmentId], [Name] FROM [Departments] WHERE EXISTS (SELECT 1 FROM [Employees] WHERE ([Employees].[DepartmentId] = [Departments].[DepartmentId]) AND ([Employees].[Salary] > 100000))";

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From("Employees")
                .Where(
                    SqlExpression.And(
                        SqlExpression.Equal(
                            new SqlColumn("Employees.DepartmentId"),
                            new SqlColumn("Departments.DepartmentId")
                        ),
                        SqlExpression.GreaterThan(new SqlColumn("Employees.Salary"), 100000)
                    )
                )
        );

        var actual = Sql.Select("DepartmentId", "Name")
            .From("Departments")
            .Where(SqlExpression.Exists(subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithNestedExists_ReturnsSqlWithNestedExists()
    {
        const string expected =
            "SELECT [CategoryId] FROM [Categories] WHERE EXISTS (SELECT 1 FROM [Products] WHERE ([Products].[CategoryId] = [Categories].[CategoryId]) AND (EXISTS (SELECT 1 FROM [OrderItems] WHERE [OrderItems].[ProductId] = [Products].[ProductId])))";

        var innerSubquery = new SqlSubquery(
            Sql.Select(1)
                .From("OrderItems")
                .Where(
                    SqlExpression.Equal(
                        new SqlColumn("OrderItems.ProductId"),
                        new SqlColumn("Products.ProductId")
                    )
                )
        );

        var outerSubquery = new SqlSubquery(
            Sql.Select(1)
                .From("Products")
                .Where(
                    SqlExpression.And(
                        SqlExpression.Equal(
                            new SqlColumn("Products.CategoryId"),
                            new SqlColumn("Categories.CategoryId")
                        ),
                        SqlExpression.Exists(innerSubquery)
                    )
                )
        );

        var actual = Sql.Select("CategoryId")
            .From("Categories")
            .Where(SqlExpression.Exists(outerSubquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsAndNotExists_ReturnsSqlWithBoth()
    {
        const string expected =
            "SELECT [UserId] FROM [Users] WHERE (EXISTS (SELECT 1 FROM [Orders] WHERE [Orders].[UserId] = [Users].[UserId])) AND (NOT EXISTS (SELECT 1 FROM [Refunds] WHERE [Refunds].[UserId] = [Users].[UserId]))";

        var ordersSubquery = new SqlSubquery(
            Sql.Select(1)
                .From("Orders")
                .Where(
                    SqlExpression.Equal(
                        new SqlColumn("Orders.UserId"),
                        new SqlColumn("Users.UserId")
                    )
                )
        );

        var refundsSubquery = new SqlSubquery(
            Sql.Select(1)
                .From("Refunds")
                .Where(
                    SqlExpression.Equal(
                        new SqlColumn("Refunds.UserId"),
                        new SqlColumn("Users.UserId")
                    )
                )
        );

        var actual = Sql.Select("UserId")
            .From("Users")
            .Where(
                SqlExpression.And(
                    SqlExpression.Exists(ordersSubquery),
                    SqlExpression.NotExists(refundsSubquery)
                )
            )
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsSubqueryWithJoin_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [CustomerId] FROM [Customers] WHERE EXISTS (SELECT 1 FROM [Orders] INNER JOIN [OrderItems] ON [Orders].[OrderId] = [OrderItems].[OrderId] WHERE ([Orders].[CustomerId] = [Customers].[CustomerId]) AND ([OrderItems].[Quantity] > 10))";

        SqlTable orders = new("Orders");
        SqlTable orderItems = new("OrderItems");

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From(orders)
                .InnerJoin(orderItems)
                .On(SqlExpression.Equal(orders + "OrderId", orderItems + "OrderId"))
                .Where(
                    SqlExpression.And(
                        SqlExpression.Equal(
                            orders + "CustomerId",
                            new SqlColumn("Customers.CustomerId")
                        ),
                        SqlExpression.GreaterThan(orderItems + "Quantity", 10)
                    )
                )
        );

        var actual = Sql.Select("CustomerId")
            .From("Customers")
            .Where(SqlExpression.Exists(subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToSql_WithExistsSubqueryWithGroupBy_ReturnsSqlWithExists()
    {
        const string expected =
            "SELECT [DepartmentId] FROM [Departments] WHERE EXISTS (SELECT 1 FROM [Employees] WHERE [Employees].[DepartmentId] = [Departments].[DepartmentId] GROUP BY ([Employees].[DepartmentId]) HAVING [Employees].[Salary] > 50000)";

        SqlTable employees = new("Employees");

        var subquery = new SqlSubquery(
            Sql.Select(1)
                .From(employees)
                .Where(
                    SqlExpression.Equal(
                        employees + "DepartmentId",
                        new SqlColumn("Departments.DepartmentId")
                    )
                )
                .GroupBy(employees + "DepartmentId")
                .Having(SqlExpression.GreaterThan(employees + "Salary", 50000))
        );

        var actual = Sql.Select("DepartmentId")
            .From("Departments")
            .Where(SqlExpression.Exists(subquery))
            .ToSql();

        Assert.Equal(expected, actual);
    }
}
