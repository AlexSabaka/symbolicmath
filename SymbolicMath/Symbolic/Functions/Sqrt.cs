using SymbolicMath.Symbolic.Operators.Scalar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    public class Sqrt : BaseFunction
    {
        public Sqrt(Expression arg)
            : base(arg) { }

        public override double Process()
        {
            return Math.Sqrt(Args[0].Process());
        }

        public override Expression Simplify()
        {
            var sa = Args[0].Simplify();
            if ((sa is Pow) && (sa as Pow).RightTerm is Constant)
            {
                var n = ((sa as Pow).RightTerm as Constant).Process();
                if (n == 2) return new Abs(sa);
                if (Math.Round(n, 10) % 2 == 0) return new Pow((sa as Pow).LeftTerm, new Constant(n / 2));
            }
            else if (sa is Constant) return new Constant(Math.Sqrt(sa.Process()));
            return new Sqrt(sa);
        }

        public override Expression Derivate(Variable dv)
        {
            throw new NotImplementedException();
        }
    }
}
