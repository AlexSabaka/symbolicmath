using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Mul : BaseOperation
    {
        public Mul(Expression left, Expression right)
            : base(Operator.Mul, left, right) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant && (lts as Constant) == Constant.Zero ||
                rts is Constant && (rts as Constant) == Constant.Zero)
                return Constant.Zero;
            else if (lts is Constant && (lts as Constant) == Constant.One) return rts;
            else if (rts is Constant && (rts as Constant) == Constant.One) return lts;

            else if (lts is Constant && (lts as Constant) == Constant.MinusOne) return -rts;
            else if (lts is Neg && (lts as Neg).LeftTerm == Constant.One) return -rts;
            else if (lts is Sub && (lts as Sub).LeftTerm == Constant.Zero) return -rts;

            else if (rts is Constant && (rts as Constant) == Constant.MinusOne) return -lts;
            else if (rts is Neg && (rts as Neg).LeftTerm == Constant.One) return -lts;

            else if (lts is Constant && rts is Constant) return new Constant(lts.Process() * rts.Process());
            else if (lts is Pow && rts is Pow)
            {
                var l1 = (lts as Pow).LeftTerm;
                var l2 = (rts as Pow).LeftTerm;

                var r1 = (lts as Pow).RightTerm;
                var r2 = (rts as Pow).RightTerm;

                if (l1 == l2) return new Pow(l1, (r1 + r2).Simplify());
            }
            return lts * rts;
        }

        public override Expression Derivate(Variable dv)
        {
            return LeftTerm.Derivate(dv) * RightTerm + LeftTerm * RightTerm.Derivate(dv);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return LeftTerm.Replace(v, expr) * RightTerm.Replace(v, expr);
        }
    }
}
