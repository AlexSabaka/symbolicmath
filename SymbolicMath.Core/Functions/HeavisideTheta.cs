using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    class HeavisideTheta : BaseFunction
    {
        public HeavisideTheta(Expression arg)
            :base(arg) { }

        public override Expression Derivate(Variable dv)
        {
            if (!this.Args[0].IsDependOf(dv)) return Constant.Zero;
            return this.Args[0].Derivate(dv) * (new DiracDelta(this.Args[0]));
        }

        public override double Process()
        {
            double value = this.Args[0].Process();
            if (value < 0) return 0;
            else if (value > 0) return 1;
            else return 0.5;
        }

        public override Expression Simplify()
        {
            return this;
        }
    }
}
