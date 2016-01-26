using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    class Ln : BaseFunction
    {
        public Ln(Expression a)
            : base(a, Constant.E)
        { }

        public override string ToString()
        {
            return string.Format("Ln[{0}]", this.Args[0].ToString());
        }

        public override double Process()
        {
            return Math.Log(this.Args[0].Process(), Math.E);
        }

        public override Expression Simplify()
        {
            var sa = this.Args[0].Simplify();
            if (sa is Symbolic.Operators.Scalar.Mul)
            {
                var msa = sa as Symbolic.Operators.Scalar.Mul;
                return (SMath.Ln(msa.LeftTerm.Simplify()) + SMath.Ln(msa.RightTerm.Simplify())).Simplify();
            }
            else if (sa is Symbolic.Operators.Scalar.Div)
            {
                var msa = sa as Symbolic.Operators.Scalar.Div;
                return (SMath.Ln(msa.LeftTerm.Simplify()) - SMath.Ln(msa.RightTerm.Simplify())).Simplify();
            }
            else if (sa is Symbolic.Constant) return new Constant(Math.Log(sa.Process()));
            else if (sa is Exp) return (sa as BaseFunction).Args[0];
            else return new Ln(sa);
        }

        public override Expression Derivate(Variable dv)
        {
            if (!this.Args[0].IsDependOf(dv)) return Constant.Zero;
            return this.Args[0].Derivate(dv) * (Constant.One / this.Args[0]);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return new Ln(Args[0].Replace(v, expr));
        }
    }
}
