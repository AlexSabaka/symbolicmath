using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    public class Tan : BaseFunction
    {
        public Tan(Expression arg)
            : base(arg) { }

        public override double Process()
        {
            return Math.Tan(Args[0].Process());
        }

        public override Expression Simplify()
        {
            throw new NotImplementedException();
        }

        public override Expression Derivate(Variable dv)
        {
            return Args[0].Derivate(dv) / (SMath.Cos(Args[0]) ^ 2);
        }
    }
}
