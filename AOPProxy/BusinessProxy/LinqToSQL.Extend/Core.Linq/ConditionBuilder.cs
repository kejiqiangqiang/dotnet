using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using FastLambda;
using System.Data.Common;

namespace BusinessProxy.LinqToSQL.Extend.Core.Linq
{
    internal class ConditionBuilder : ExpressionVisitor
    {
        DbCommand _DbCommand = null;
        public DbCommand DbCommand
        {
            get { return _DbCommand; }
        }
        private Stack<string> m_conditionParts;
        private string Condition;

        public void Build(Expression expression, DbCommand dbCommand)
        {
            this._DbCommand = dbCommand;
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);
            this.m_conditionParts = new Stack<string>();
            this.Visit(evaluatedExpression);
            this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
            this._DbCommand.CommandText = this.Condition;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null) return b;
            string opr;
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    opr = "AND";
                    break;
                case ExpressionType.OrElse:
                    opr = "OR";
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                default:
                    throw new NotSupportedException(b.NodeType + " is not supported.");
            }
            base.Visit(b.Left);
            base.Visit(b.Right);

            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();

            string condition = String.Format("({0} {1} {2})", left, opr, right);
            this.m_conditionParts.Push(condition);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c == null) return c;
            string condition = String.Format("@p{0}", this._DbCommand.Parameters.Count);
            DbParameter ps = _DbCommand.CreateParameter();
            ps.Value = c.Value;
            ps.ParameterName = "p" + _DbCommand.Parameters.Count;

            this._DbCommand.Parameters.Add(ps);
            this.m_conditionParts.Push(condition);
            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null) return m;
            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null) return m;
            this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));
            return m;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m == null) return m;
            string format;
            switch (m.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(m.NodeType + " is not supported!");
            }
            this.Visit(m.Object);
            this.Visit(m.Arguments[0]);
            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();
            this.m_conditionParts.Push(String.Format(format, left, right));
            return m;
        }
    }
}
