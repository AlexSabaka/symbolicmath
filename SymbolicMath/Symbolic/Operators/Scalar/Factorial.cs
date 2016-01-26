using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Factorial : BaseOperation
    {
        public Factorial(Expression term)
            : base(Operator.Factorial, term) { }

        public override Expression Simplify()
        {
            throw new NotImplementedException();
        }

        public override Expression Derivate(Variable dv)
        {
            throw new NotImplementedException();
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            throw new NotImplementedException();
        }
    }
}
