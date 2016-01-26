using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Sub : BaseOperation
    {
        public Sub(Expression left, Expression right)
            : base(Operator.Sub, left, right) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant && rts is Constant) return new Constant(lts.Process() - rts.Process());
            else if (lts is Constant && (lts as Constant) == Constant.Zero) return -rts;
            else if (rts is Constant && (rts as Constant) == Constant.Zero) return lts;
            else if (lts is Neg && (lts as Neg).LeftTerm == rts) return -2 * rts;
            else return lts - rts;
        }

        public override Expression Derivate(Variable dv)
        {
            return LeftTerm.Derivate(dv) - RightTerm.Derivate(dv);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return LeftTerm.Replace(v, expr) - RightTerm.Replace(v, expr);
        }
    }
}
