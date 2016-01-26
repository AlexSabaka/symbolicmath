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
            this.Operator = op;
            this.LeftTerm = left;
            this.RightTerm = right;
        }

        public BaseOperation(Operator op, Expression unaryTerm)
        {
            if (!op.IsUnary) throw new InvalidOperationException();
            this.Operator = op;
            this.LeftTerm = unaryTerm;
        }

        public override double Process()
        {
            if (this.RightTerm == null) return this.Operator.Apply(this.LeftTerm.Process());
            else return this.Operator.Apply(this.LeftTerm.Process(), this.RightTerm.Process());
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
            //if (this.Operator == null && this is Neg)
            //    return "-" + this.LeftTerm.ToString();
            //if (this.Operator == null && this is Pos)
            //    return "-" + this.LeftTerm.ToString();
            return string.Format(this.Operator.CreateFormat(), this.LeftTerm, this.RightTerm);
        }
    }
}
