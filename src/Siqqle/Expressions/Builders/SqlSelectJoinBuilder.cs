using Siqqle.Syntax;

namespace Siqqle.Expressions.Builders
{
    internal class SqlSelectJoinBuilder : ISqlSelectJoinSyntax
    {
        public SqlSelectJoinBuilder(SqlSelectFromBuilder builder, SqlJoin join)
        {
            Builder = builder;
            Join = join;
        }

        public SqlSelectFromBuilder Builder 
        { 
            get; 
        }

        public SqlJoin Join
        {
            get;
        }

        public ISqlSelectJoinOnSyntax On(SqlExpression predicate)
        {
            Join.On = new SqlOn(predicate);
            return Builder;
        }
    }
}