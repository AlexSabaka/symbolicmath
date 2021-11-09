using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Pow : BaseOperation
    {
        public Pow(Expression left, Expression right)
            : base(Operator.Pow, left, right) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant && (lts as Constant) == Constant.Zero &&
                rts is Constant && (rts as Constant) == Constant.Zero)
                return Constant.NaN;
            else if (lts is Constant && (lts as Constant) == Constant.Zero) return Constant.Zero;
            else if (rts is Constant && (rts as Constant) == Constant.Zero) return Constant.One;
            else if (rts is Constant && (rts as Constant) == Constant.One) return lts;
            else if (lts is Constant && (lts as Constant) == Constant.One) return Constant.One;
            else if (lts is Constant && rts is Constant) return new Constant(Math.Pow(lts.Process(), rts.Process()));
            else if (lts is Pow) return new Pow((lts as Pow).LeftTerm, ((lts as Pow).LeftTerm + RightTerm).Simplify());
            else if (lts is Mul)
            {
                var mlts = lts as Mul;
                return ((mlts.LeftTerm ^ rts).Simplify() * (mlts.RightTerm ^ rts).Simplify()).Simplify();
            }
            else if (rts is Constant && lts is Neg)
            {
                var n = (rts as Constant).Process();
                if (Math.Round(n, 10) % 2 == 0) return (lts as Neg).LeftTerm ^ rts;
            }
            return new Pow(lts, rts);
        }

        public override Expression Derivate(Variable dv)
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant) return ((lts ^ rts) * SMath.Ln(lts)).Simplify();
            else if (rts is Constant) return (rts * lts.Derivate(dv) * (lts ^ (rts - 1))).Simplify();
            else return (SMath.Ln(lts) * rts).Derivate(dv).Simplify() * (lts ^ rts);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return LeftTerm.Replace(v, expr) ^ RightTerm.Replace(v, expr);
        }
    }
}
