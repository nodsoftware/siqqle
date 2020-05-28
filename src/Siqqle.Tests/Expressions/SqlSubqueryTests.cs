using System;
using Siqqle.Expressions.Builders;
using Siqqle.Syntax;
using Xunit;

namespace Siqqle.Expressions.Tests
{
    public class SqlSubqueryTests
    {
        [Fact]
        public void Ctor_WithNullQuery_ThrowsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlSubquery((SqlSelect)null, null));
            Assert.Throws<ArgumentNullException>(() => new SqlSubquery((ISqlSyntaxEnd<SqlSelect>)null, null));
            Assert.Throws<ArgumentNullException>(() => new SqlSubquery(null));
        }

        [Fact]
        public void Ctor_WithQueryAndAlias_SetsQueryProperty()
        {
            var query = Sql.Select("Id", "Name").From("Users").Go();
            var subquery = new SqlSubquery(query, "u");

            Assert.NotNull(subquery.Query);
            Assert.Same(query, subquery.Query);
        }
    }
}