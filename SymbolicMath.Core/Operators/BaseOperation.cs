using SymbolicMath.Symbolic.Operators.Scalar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators
{
    public abstract class BaseOperation : Expression
    {
        public Expression LeftTerm { get; set; }
        public Expression RightTerm { get; set; }

        public Operator Operator { get; set; }

        public BaseOperation(Operator op, Expression left, Expression right)
        {
            Operator = op;
            LeftTerm = left;
            RightTerm = right;
        }

        public BaseOperation(Operator op, Expression unaryTerm)
        {
            if (!op.IsUnary) throw new InvalidOperationException();
            Operator = op;
            LeftTerm = unaryTerm;
        }

        public override double Process()
        {
            if (RightTerm == null) return Operator.Apply(LeftTerm.Process());
            else return Operator.Apply(LeftTerm.Process(), RightTerm.Process());
        }

        public abstract override Expression Simplify();

        public abstract override Expression Derivate(Variable dv);

        public override bool IsDependOf(Variable var)
        {
            return LeftTerm.IsDependOf(var) || RightTerm.IsDependOf(var);
        }

        public override int GetHashCode()
        {
            return LeftTerm.GetHashCode() + (RightTerm ?? 0).GetHashCode() + (Operator ?? Operator.Pos).GetHashCode();
        }

        public override string ToString()
        {
            //if (Operator == null && this is Neg)
            //    return "-" + LeftTerm.ToString();
            //if (Operator == null && this is Pos)
            //    return "-" + LeftTerm.ToString();
            return string.Format(Operator.CreateFormat(), LeftTerm, RightTerm);
        }
    }
}
