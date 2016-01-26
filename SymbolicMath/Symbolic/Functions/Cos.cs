using SymbolicMath.Symbolic.Operators.Scalar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    class Cos : BaseFunction
    {
        public Cos(Expression arg)
            : base(arg) { }

        public override double Process()
        {
            return Math.Cos(this.Args[0].Process());
        }

        public override Expression Simplify()
        {
            var sa = this.Args[0].Simplify();
            if (sa is Constant && (sa as Constant) == Constant.Zero) return Constant.One;
            else if (sa is Neg) return -SMath.Cos((sa as Neg).LeftTerm);
            else if (sa is Mul)
            {
                var lt = (sa as Mul).LeftTerm;
                var rt = (sa as Mul).RightTerm;
                if (lt is Constant)
                {
                    var n = (lt as Constant).Process();
                    if (Math.Round(n, 10) % 2 == 0)
                    {
                        Expression arg = (new Constant(n / 2) * rt).Simplify();
                        return (SMath.Cos(arg).Simplify() ^ 2) - (SMath.Sin(arg).Simplify() ^ 2).Simplify();
                    }
                }
                else if (rt is Constant)
                {
                    var n = (rt as Constant).Process();
                    if (Math.Round(n, 10) % 2 == 0)
                    {
                        Expression arg = (new Constant(n / 2) * lt).Simplify();
                        return (SMath.Cos(arg).Simplify() ^ 2) - (SMath.Sin(arg).Simplify() ^ 2).Simplify();
                    }
                }
            }
            return SMath.Cos(sa);
        }

        public override Expression Derivate(Variable dv)
        {
            if (!this.Args[0].IsDependOf(dv)) return Constant.Zero;
            return -(this.Args[0].Derivate(dv)) * (new Sin(this.Args[0]));
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return new Cos(Args[0].Replace(v, expr));
        }
    }
}
