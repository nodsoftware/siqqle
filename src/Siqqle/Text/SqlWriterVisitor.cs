using System;
using System.Collections.Generic;
using System.Linq;
using Siqqle.Expressions;
using Siqqle.Expressions.Visitors;

namespace Siqqle.Text
{
    public class SqlWriterVisitor : SqlVisitor
    {
        private readonly SqlWriter _writer;

        public SqlWriterVisitor(SqlWriter writer)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public Action<SqlParameter> ParameterVisited
        {
            get;
            set;
        }

        protected SqlWriter Writer
        {
            get;
        }

        public override void Visit(SqlGroupBy expression)
        {
            Writer.WriteKeyword(SqlKeywords.GroupBy);
        }

        public override void Visit(SqlHaving expression)
        {
            Writer.WriteKeyword(SqlKeywords.Having);
        }

        public override void Visit(SqlParameter expression)
        {
            Writer.WriteParameter(expression.ParameterName);
            ParameterVisited?.Invoke(expression);
        }

        public override void Visit(SqlSelect expression)
        {
            Writer.WriteKeyword(SqlKeywords.Select);
        }

        public override void Visit(SqlDataType expression)
        {
            Writer.WriteDataType(expression);
        }

        public override void Visit(SqlDelete expression)
        {
            Writer.WriteKeyword(SqlKeywords.Delete);
        }

        public override void Visit(SqlLimit expression)
        {
            Writer.WriteLimit(expression.Offset, expression.Count);
        }

        public override void Visit(SqlSubquery expression)
        {
            // TODO: This is weird and should actually be handled in SqlSubquery.Accept().
            Writer.WriteOpenParenthesis();
            expression.Query.Accept(this);
            Writer.WriteCloseParenthesis();
            if (expression.Alias != null)
            {
                Writer.WriteIdentifier(expression.Alias);
            }
        }

        public override void Visit(SqlTable expression)
        {
            Writer.WriteIdentifier(expression.TableName);
            if (expression.Alias != null)
            {
                Writer.WriteIdentifier(expression.Alias);
            }
        }

        public override void Visit(SqlUpdate expression)
        {
            Writer.WriteKeyword(SqlKeywords.Update);
        }

        public override void Visit(SqlCast expression)
        {
            Writer.WriteKeyword(SqlKeywords.Cast);
            Writer.WriteRaw("(");
            Writer.ClearPendingSpace();
            Visit(expression.Value);
            Writer.WriteKeyword(SqlKeywords.As);
            Visit(expression.DataType);
            Writer.WriteCloseParenthesis();
        }

        public override void Visit(SqlColumn expression)
        {
            Writer.WriteIdentifier(expression.ColumnName);
            if (expression.Alias != null)
            {
                Writer.WriteKeyword(SqlKeywords.As);
                Writer.WriteIdentifier(expression.Alias);
            }
        }

        public override void Visit(SqlJoin expression)
        {
            switch (expression.Type)
            {
                case SqlJoinType.Inner:
                    Writer.WriteKeyword(SqlKeywords.Inner);
                    break;

                case SqlJoinType.Left:
                    Writer.WriteKeyword(SqlKeywords.Left);
                    break;

                case SqlJoinType.Right:
                    Writer.WriteKeyword(SqlKeywords.Right);
                    break;
            }
            Writer.WriteKeyword(SqlKeywords.Join);
        }

        public override void Visit(SqlOn expression)
        {
            Writer.WriteKeyword(SqlKeywords.On);
        }

        public override void Visit(SqlFrom expression)
        {
            Writer.WriteKeyword(SqlKeywords.From);
        }

        public override void Visit(SqlFunction expression)
        {
            Writer.Write(expression.FunctionName);
            Writer.WriteRaw("(");
            Writer.ClearPendingSpace();
            expression.Arguments?.Accept(this);
            Writer.WriteCloseParenthesis();
            if (expression.Alias != null)
            {
                Writer.WriteKeyword(SqlKeywords.As);
                Writer.WriteIdentifier(expression.Alias);
            }
        }

        public override void Visit(SqlValueList expression)
        {
            Writer.WriteOpenParenthesis();
            expression.Values.Accept(this);
            Writer.WriteCloseParenthesis();
        }

        public override void Visit(SqlUnion expression)
        {
            expression.Statements.For(
                (index, statement) =>
                {
                    if (index > 0) Writer.WriteKeyword(SqlKeywords.Union);
                    Writer.WriteOpenParenthesis();
                    statement.Accept(this);
                    Writer.WriteCloseParenthesis();
                });
        }
        

        public override void Visit(SqlBatch expression)
        {
            expression.Statements.For(
                (index, statement) =>
                {
                    if (index > 0) Writer.WriteRaw(";");
                    statement.Accept(this);
                });
        }

        public override void Visit(SqlValues expression)
        {
            Writer.WriteKeyword(SqlKeywords.Values);
        }

        public override void Visit(SqlWhere expression)
        {
            Writer.WriteKeyword(SqlKeywords.Where);
        }

        public override void Visit(IEnumerable<SqlColumn> expressions)
        {
            Writer.WriteOpenParenthesis();
            expressions.For(
                (index, expression) =>
                {
                    if (index > 0) Writer.WriteRaw(",");
                    expression.Accept(this);
                });
            Writer.WriteCloseParenthesis();
        }

        public override void Visit(SqlInto expression)
        {
            Writer.WriteKeyword(SqlKeywords.Into);
        }

        public override void Visit(IEnumerable<SqlValue> expressions)
        {
            expressions.For(
                (index, expression) =>
                {
                    if (index > 0) Writer.WriteRaw(",");
                    expression.Accept(this);
                });
        }

        public override void Visit(IEnumerable<SqlSort> expressions)
        {
            expressions.For(
                (index, expression) =>
                {
                    if (index > 0) Writer.WriteRaw(",");
                    expression.Accept(this);
                });
        }

        public override void Visit(IEnumerable<SqlAssign> expressions)
        {
            expressions.For(
                (index, expression) =>
                {
                    if (index > 0) Writer.WriteRaw(",");
                    expression.Accept(this);
                });
        }

        public override void Visit(IEnumerable<IEnumerable<SqlValue>> expressions)
        {
            expressions.For(
                (index, expression) =>
                {
                    if (index > 0) Writer.WriteRaw(",");
                    Writer.WriteOpenParenthesis();
                    expression.Accept(this);
                    Writer.WriteCloseParenthesis();
                });
        }

        public override void Visit(SqlOrderBy expression)
        {
            Writer.WriteKeyword(SqlKeywords.OrderBy);
        }

        public override void Visit(SqlBinaryOperator @operator)
        {
            Writer.WriteOperator(@operator);
        }

        public override void Visit(SqlSortOrder sortOrder)
        {
            Writer.WriteSortOrder(sortOrder);
        }

        public override void Visit(SqlSet expression)
        {
            Writer.WriteKeyword(SqlKeywords.Set);
        }

        public override void Visit(SqlBinaryExpression expression)
        {
            bool useParentheses = expression.Operator == SqlBinaryOperator.And ||
                                  expression.Operator == SqlBinaryOperator.Or;

            // TODO: The accept code should be in the class itself.
            if (useParentheses) Writer.WriteOpenParenthesis();
            expression.Left.Accept(this);
            if (useParentheses) Writer.WriteCloseParenthesis();
            expression.Operator.Accept(this);
            if (useParentheses) Writer.WriteOpenParenthesis();
            expression.Right.Accept(this);
            if (useParentheses) Writer.WriteCloseParenthesis();
        }

        public override void Visit(SqlConstant expression)
        {
            Writer.WriteValue(expression.Value);
        }

        public override void Visit(SqlInsert expression)
        {
            Writer.WriteKeyword(SqlKeywords.Insert);
        }
    }
}
