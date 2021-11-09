using SymbolicMath.Symbolic.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public static class SMath
    {
        public static Expression Sqrt(Expression a)
        {
            return new Sqrt(a);
        }

        public static Expression Sin(Expression a)
        {
            return new Sin(a);
        }

        public static Expression ArcSin(Expression a)
        {
            return new ArcSin(a);
        }

        public static Expression Cos(Expression a)
        {
            return new Cos(a);
        }

        public static Expression ArcCos(Expression a)
        {
            return new ArcCos(a);
        }

        public static Expression Tan(Expression a)
        {
            return new Tan(a);
        }

        public static Expression ArcTan(Expression a)
        {
            return new ArcTan(a);
        }

        public static Expression Exp(Expression a)
        {
            return new Exp(a);
        }

        public static Expression Log(Expression arg, Expression logBase)
        {
            return (new Ln(arg)) / (new Ln(logBase));
        }

        public static Expression Ln(Expression arg)
        {
            return new Ln(arg);
        }
    }
}
