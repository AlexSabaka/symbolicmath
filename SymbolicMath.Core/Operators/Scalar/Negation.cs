using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Neg : BaseOperation
    {
        public Neg(Expression term)
            : base(Operator.Neg, term) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            if (lts is Constant && (lts as Constant) == Constant.Zero) return Constant.Zero;
            else return -lts;
        }

        public override Expression Derivate(Variable dv)
        {
            return -LeftTerm.Derivate(dv);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return -LeftTerm.Replace(v, expr);
        }
    }
}
