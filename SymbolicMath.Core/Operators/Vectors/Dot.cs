using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Vectors
{
    class Dot : BaseVectorOperation
    {
        public Dot(Vector left, Vector right)
            : base(Operator.Dot, left, right) { }

        public override Vector Derivate(Variable dv)
        {
            throw new NotImplementedException();
        }

        public override Vector Simplify()
        {
            throw new NotImplementedException();
        }

    }
}
