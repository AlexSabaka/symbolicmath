using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    class Abs : BaseFunction
    {
        public Abs(Expression arg)
            : base(arg) { }

        public override Expression Derivate(Variable dv)
        {
            var sa = this.Args[0];
            return sa.Derivate(dv) * (sa / new Abs(sa));
        }

        public override double Process()
        {
            return Math.Abs(this.Args[0].Process());
        }

        public override Expression Simplify()
        {
            var sa = this.Args[0].Simplify();
            if (sa is Symbolic.Operators.Scalar.Neg) return new Abs((sa as Symbolic.Operators.Scalar.Neg).LeftTerm);
            else return new Abs(sa);
        }

        public override string ToString()
        {
            return string.Format("|{0}|", this.Args[0]);
        }
    }
}
