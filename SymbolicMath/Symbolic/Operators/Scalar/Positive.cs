using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Pos : BaseOperation
    {
        public Pos(Expression term)
            : base(Operator.Pos, term) { }

        public override Expression Simplify()
        {
            return LeftTerm.Simplify();
        }

        public override Expression Derivate(Variable dv)
        {
            return LeftTerm.Derivate(dv);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return +LeftTerm.Replace(v, expr);
        }
    }
}
