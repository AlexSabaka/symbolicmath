using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    public class Exp : BaseFunction
    {
        public Exp(Expression arg)
            : base(arg) { }

        public override double Process()
        {
            return Math.Exp(this.Args[0].Process());
        }

        public override Expression Simplify()
        {
            var sa = this.Args[0].Simplify();
            if (sa is Ln) return (sa as BaseFunction).Args[0];
            else return new Exp(sa);
        }

        public override Expression Derivate(Variable dv)
        {
            if (!this.Args[0].IsDependOf(dv)) return Constant.Zero;
            return (this.Args[0].Derivate(dv)) * this;
        }
    }
}
