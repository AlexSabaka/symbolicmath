using SymbolicMath.Symbolic.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators.Scalar
{
    public class Div : BaseOperation
    {
        public Div(Expression left, Expression right)
            : base(Operator.Div, left, right) { }

        public override Expression Simplify()
        {
            var lts = LeftTerm.Simplify();
            var rts = RightTerm.Simplify();
            if (lts is Constant && (lts as Constant) == Constant.Zero) return Constant.Zero;
            else if (rts is Constant && (rts as Constant) == Constant.Zero) return Constant.Inf;
            else if (lts is Constant && (lts as Constant) == Constant.Zero &&
                     rts is Constant && (rts as Constant) == Constant.Zero) return Constant.NaN;
            else if (rts is Constant && (rts as Constant) == Constant.MinusOne) return -lts;
            else if (rts == lts) return Constant.One;
            else if (lts is Constant && rts is Constant) return new Constant(lts.Process() / rts.Process());
            else if (lts is Pow && rts is Pow)
            {
                var l1 = (lts as Pow).LeftTerm;
                var l2 = (rts as Pow).LeftTerm;
                
                var r1 = (lts as Pow).RightTerm;
                var r2 = (rts as Pow).RightTerm;

                if (l1 == l2) return new Pow(l1, (r1 - r2).Simplify());
            }
            else if (lts is Sin && rts is Cos)
            {
                var a1 = (lts as Sin).Args[0];
                var a2 = (rts as Cos).Args[0];
                if (a1 == a2) return SMath.Tan(a1);
            }
            else if (lts is Cos && rts is Sin)
            {
                var a1 = (rts as Sin).Args[0];
                var a2 = (lts as Cos).Args[0];
                if (a1 == a2) return 1 / SMath.Tan(a1);
            }
            return lts / rts;
        }

        public override Expression Derivate(Variable dv)
        {
            return (LeftTerm.Derivate(dv) * RightTerm - LeftTerm * RightTerm.Derivate(dv)) / (RightTerm ^ 2);
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return LeftTerm.Replace(v, expr) / RightTerm.Replace(v, expr);
        }
    }
}