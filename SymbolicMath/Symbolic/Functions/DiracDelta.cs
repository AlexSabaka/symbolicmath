using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    class DiracDelta : BaseFunction
    {
        public DiracDelta(Expression arg)
            : base(arg) { }

        public override Expression Derivate(Variable dv)
        {
            throw new NotImplementedException();
        }

        public override double Process()
        {
            double value = this.Args[0].Process();
            return value == 0 ? double.PositiveInfinity : 0;
        }

        public override Expression Simplify()
        {
            return this;
        }
    }
}
