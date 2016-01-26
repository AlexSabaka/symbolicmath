using SymbolicMath.Symbolic.Operators.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators
{
    public abstract class BaseVectorOperation : Vector
    {
        public Vector LeftTerm { get; set; }
        public Vector RightTerm { get; set; }

        public Operator Operator { get; set; }

        public BaseVectorOperation()
        {

        }

        public BaseVectorOperation(Operator op, Vector left, Vector right)
        {
            if (!op.IsVectorOperator) throw new InvalidOperationException();
            this.Operator = op;
            this.LeftTerm = left;
            this.RightTerm = right;
        }

        public BaseVectorOperation(Operator op, Vector unaryTerm)
        {
            if (!op.IsUnary || !op.IsVectorOperator) throw new InvalidOperationException();
            this.Operator = op;
            this.LeftTerm = unaryTerm;
        }

        public new double[] Process()
        {
            return new[] { 0.0 };
            //if (this.RightTerm == null) return this.Operator.Apply(this.LeftTerm.Process());
            //else return this.Operator.Apply(this.LeftTerm.Process(), this.RightTerm.Process());
        }

        public abstract new Vector Simplify();

        public abstract new Vector Derivate(Variable dv);

        public override bool IsDependOf(Variable var)
        {
            return LeftTerm.IsDependOf(var) || RightTerm.IsDependOf(var);
        }

        public override int GetHashCode()
        {
            return LeftTerm.GetHashCode() + (RightTerm ?? Vector.Zero(1)).GetHashCode() + (Operator ?? Operator.Pos).GetHashCode();
        }

        public override string ToString()
        {
            if (this.Operator == null && this is Neg)
                return "-" + this.LeftTerm.ToString();
            if (this.Operator == null && this is Pos)
                return "-" + this.LeftTerm.ToString();
            return string.Format(this.Operator.CreateFormat(), this.LeftTerm, this.RightTerm);
        }
    }
}
