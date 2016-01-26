using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    public class ArcTan : BaseFunction
    {
        public ArcTan(Expression arg)
            : base(arg) { }

        public override double Process()
        {
            return Math.Atan(Args[0].Process());
        }

        public override Expression Simplify()
        {
            throw new NotImplementedException();
        }

        public override Expression Derivate(Variable dv)
        {
            return (Args[0].Derivate(dv)) / (1 - Args[0] ^ 2);
        }
    }
}
