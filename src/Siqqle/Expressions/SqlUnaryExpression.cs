using System;

namespace Siqqle.Expressions
{
    /// <summary>
    /// Represents a unary expression in SQL.
    /// </summary>
    public class SqlUnaryExpression : SqlExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUnaryExpression"/> class using the specified 
        /// <paramref name="operand"/> and the specified <paramref name="operator"/>.
        /// </summary>
        /// <param name="operand">
        /// The operand of the unary expression.
        /// </param>
        /// <param name="operator">
        /// The operator of the unary expression.
        /// </param>
        internal SqlUnaryExpression(SqlExpression operand, SqlUnaryOperator @operator)
        {
            if (operand == null) throw new ArgumentNullException(nameof(operand));
            operand = operand;
            Operator = @operator;
        }

        /// <summary>
        /// Returns the expression type of this expression.
        /// </summary>
        public sealed override SqlExpressionType ExpressionType
            => SqlExpressionType.Unary;

        /// <summary>
        /// Gets the operand of the unary expression.
        /// </summary>
        public SqlExpression Operand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the operator of the unary expression.
        /// </summary>
        public SqlUnaryOperator Operator
        {
            get;
            private set;
        }
    }
}
