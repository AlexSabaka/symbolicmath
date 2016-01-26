using SymbolicMath.Symbolic.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Add : BaseOperation
    {
        public Add(Expression left, Expression right)
            : base(Operator.Add, left, right) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant && rts is Constant) return new Constant(lts.Process() + rts.Process());
            else if (lts is Constant && (lts as Constant) == Constant.Zero) return rts;
            else if (rts is Constant && (rts as Constant) == Constant.Zero) return lts;
            else if (lts is Pow && rts is Pow)
            {
                var l1 = (lts as Pow).LeftTerm;
                var l2 = (rts as Pow).LeftTerm;

                var r1 = (lts as Pow).RightTerm;
                var r2 = (rts as Pow).RightTerm;

                if ((l1 is Sin && l2 is Cos) ||
                    (l1 is Cos && l2 is Sin))
                {
                    var a1 = (l1 as BaseFunction).Args[0];
                    var a2 = (l2 as BaseFunction).Args[0];

                    if (a1 == a2 && r1 == Constant.Two && r2 == Constant.Two) return Constant.One;
                }
            }
            else if (lts is Mul && rts is Mul)
            {
                var llm = (lts as Mul).LeftTerm;
                var lrm = (lts as Mul).RightTerm;
                var rlm = (rts as Mul).LeftTerm;
                var rrm = (rts as Mul).RightTerm;
                if (llm == rlm) return llm.Simplify() * (lrm + rrm).Simplify();
                else if (lrm == rrm) return lrm.Simplify() * (llm + rlm).Simplify();
                else if (llm == rrm) return llm.Simplify() * (lrm + rlm).Simplify();
                else if (lrm == rlm) return lrm.Simplify() * (llm + rrm).Simplify();
            }
            return lts + rts;
        }

        public override Expression Derivate(Variable dv)
        {
            return LeftTerm.Derivate(dv) + RightTerm.Derivate(dv);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return LeftTerm.Replace(v, expr) + RightTerm.Replace(v, expr);
        }
    }

}
