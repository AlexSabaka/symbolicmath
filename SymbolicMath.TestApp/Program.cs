using SymbolicMath.Symbolic;
using SymbolicMath.Symbolic.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.ComponentModel.DataAnnotations;

namespace SymbolicMath.TestApp
{
    public class Program
    {
        static bool IsAddOp(Expression expr)
        {
            return expr is Symbolic.Operators.Scalar.Add || expr is Symbolic.Operators.Scalar.Sub;
        }

        static bool IsMulOp(Expression expr)
        {
            return expr is Symbolic.Operators.Scalar.Mul || expr is Symbolic.Operators.Scalar.Div;
        }

        static bool IsBinary(Expression expr)
        {
            return (expr is Symbolic.Operators.BaseOperation) && !(expr as Symbolic.Operators.BaseOperation).Operator.IsUnary;
        }

        static Expression Left(Expression expr)
        {
            if (IsBinary(expr)) return (expr as Symbolic.Operators.BaseOperation).LeftTerm;
            else return null;
        }

        static Expression Right(Expression expr)
        {
            if (IsBinary(expr)) return (expr as Symbolic.Operators.BaseOperation).RightTerm;
            else return null;
        }

        static Tuple<Expression, Expression, Expression> IsSquareEquation(Expression left)
        {
            if (IsAddOp(left))
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (IsAddOp(ll) && !IsAddOp(lr))
                {
                    Expression lll = Left(ll);
                    Expression llr = Right(ll);
                    return null;
                }
                else if (!IsAddOp(ll) && IsAddOp(lr))
                {
                    Expression lrl = Left(lr);
                    Expression lrr = Right(lr);
                    return null;
                }
                else return null;
            }
            else return null;
        }

        static Expression Solve(Expression left, Expression right, Variable v)
        {
            left = left.Simplify();
            right = right.Simplify();
            Console.WriteLine("\t{0} = {1} // {2} == {3}", left, right, left.GetType().Name, right.GetType().Name);

            if (!left.IsDependOf(v)) throw new Exception("Can't solve equation. No dependencies of variable '" + v.Name + "'.");
            if (left is Symbolic.Operators.Scalar.Add)
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (lr.IsDependOf(v)) return Solve(lr, right - ll, v);
                else return Solve(ll, right - lr, v);
            }
            else if (left is Symbolic.Operators.Scalar.Sub)
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (lr.IsDependOf(v)) return Solve(-lr, right - ll, v);
                else return Solve(ll, right + lr, v);
            }
            else if (left is Symbolic.Operators.Scalar.Div)
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (lr.IsDependOf(v)) return Solve(lr, ll / right, v);
                else return Solve(ll, right * lr, v);
            }
            else if (left is Symbolic.Operators.Scalar.Mul)
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (lr.IsDependOf(v)) return Solve(lr, right / ll, v);
                else return Solve(ll, right / lr, v);
            }
            else if (left is Symbolic.Operators.Scalar.Pow)
            {
                Expression ll = Left(left);
                Expression lr = Right(left);
                if (lr.IsDependOf(v)) return Solve(lr, SMath.Log(right, ll), v);
                else return Solve(ll, right ^ (1 / lr), v);
            }
            else if (left is Symbolic.Functions.Sqrt)
            {
                Expression ll = (left as Symbolic.Functions.Sqrt).Args[0];
                return Solve(ll, right ^ 2, v);
            }
            else if (left is Symbolic.Functions.Sin)
            {
                Expression ll = (left as Symbolic.Functions.Sin).Args[0];
                return Solve(ll, SMath.ArcSin(right), v);
            }
            else if (left is Symbolic.Functions.Cos)
            {
                Expression ll = (left as Symbolic.Functions.Cos).Args[0];
                return Solve(ll, SMath.ArcCos(right), v);
            }
            else if (left is Symbolic.Functions.Tan)
            {
                Expression ll = (left as Symbolic.Functions.Tan).Args[0];
                return Solve(ll, SMath.ArcTan(right), v);
            }
            else if (left is Symbolic.Functions.ArcSin)
            {
                Expression ll = (left as Symbolic.Functions.ArcSin).Args[0];
                return Solve(ll, SMath.Sin(right), v);
            }
            else if (left is Symbolic.Functions.ArcCos)
            {
                Expression ll = (left as Symbolic.Functions.ArcCos).Args[0];
                return Solve(ll, SMath.Cos(right), v);
            }
            else if (left is Symbolic.Functions.ArcTan)
            {
                Expression ll = (left as Symbolic.Functions.ArcTan).Args[0];
                return Solve(ll, SMath.Tan(right), v);
            }
            else if (left is Symbolic.Functions.Exp)
            {
                Expression ll = (left as Symbolic.Functions.Exp).Args[0];
                return Solve(ll, SMath.Ln(right), v);
            }
            else if (left is Symbolic.Functions.Ln)
            {
                Expression ll = (left as Symbolic.Functions.Ln).Args[0];
                return Solve(ll, SMath.Exp(right), v);
            }
            else if (left is Variable && left.IsDependOf(v)) return right;
            else throw new Exception("Can't solve equation. Unknown equation type.");
        }

        static double NSolve(Expression f, Variable v, double v0 = 0, double eps = 1e-6)
        {
            double vn1 = v0;
            Expression phi = v - f / f.Derivate(v).Simplify();
            v.Value = vn1;
            do
            {
                vn1 = phi.Process();
                v.Value = vn1;
            } while (Math.Abs(f.Process()) > eps);
            return vn1;
        }

        static double NIntegrate(Expression f, Variable v, double a, double b, double eps = 1e-2)
        {
            double res = 0;
            Expression df = f.Derivate(v).Simplify();
            v.Value = a;
            for (double pv = a, cv = a + eps, step = eps;
                cv < b + step / 2;
                v.Value = cv, step = eps / (1 + Math.Abs(df.Process())), pv = cv, cv += step)
            {
                v.Value = (pv + cv) / 2;
                res += step * f.Process();
            }
            return res;
        }

        static Expression LaplaceTransform(Expression expr, Variable t, Variable s)
        {

            return 0;
        }

        static Expression Laplace(Expression expr, Variable x, Variable y, Variable z)
        {
            return expr.Derivate(x, 2).Simplify() + expr.Derivate(y, 2).Simplify() + expr.Derivate(z, 2).Simplify();
        }

        static Expression Subs(Expression expr, Expression a, Expression b)
        {
            if (expr == a) return b;
            else if (expr is Symbolic.Functions.BaseFunction)
            {
                for (int i = 0; i < (expr as Symbolic.Functions.BaseFunction).Args.Length; ++i)
                    (expr as Symbolic.Functions.BaseFunction).Args[i] = Subs((expr as Symbolic.Functions.BaseFunction).Args[i], a, b);
            }
            else if (expr is Symbolic.Operators.BaseOperation)
            {
                (expr as Symbolic.Operators.BaseOperation).LeftTerm = Subs((expr as Symbolic.Operators.BaseOperation).LeftTerm, a, b);
                (expr as Symbolic.Operators.BaseOperation).RightTerm = Subs((expr as Symbolic.Operators.BaseOperation).RightTerm, a, b);
            }
            return expr;
        }

        static Expression Integrate(Expression expr, Variable v)
        {
            if (IsAddOp(expr))
            {
                var l = Left(expr);
                var r = Right(expr);
                if (expr is Symbolic.Operators.Scalar.Add) return Integrate(l, v) + Integrate(r, v);
                else return Integrate(l, v) - Integrate(r, v);
            }
            else if (expr is Symbolic.Operators.Scalar.Mul)
            {
                if (!Left(expr).IsDependOf(v)) return Left(expr) * Integrate(Right(expr), v);
                else if (!Right(expr).IsDependOf(v)) return Right(expr) * Integrate(Left(expr), v);
                else throw new NotImplementedException(); //Partial integration
            }
            else return TableIntegral(expr, v);
        }

        static Expression Arg(Expression expr)
        {
            if (expr is Symbolic.Functions.BaseFunction) return (expr as Symbolic.Functions.BaseFunction).Args[0];
            else return null;
        }

        static Expression TableIntegral(Expression expr, Variable v)
        {
            if (expr is Constant)
            {
                if (expr == Constant.Zero) return Constant.One;
                else return expr * v;
            }
            //else if (expr is Symbolic.Functions.Sin) ;
            else throw new NotImplementedException();
        }

        public static void Main(string[] args)
        {
            var x = new Variable("x");
            var y = new Variable("y");
            var z = new Variable("z");
            var u = new Variable("u");
            
            var f = SMath.Cos(x * y) - x * SMath.Sin(y * z);
            Console.WriteLine(f);
            Console.WriteLine(f.Derivate(x).Derivate(y).Simplify());

            var v1 = new Vector(x, y, z);
            var v2 = new Vector(-x, SMath.Sin(y), SMath.Cos(z));
            Console.WriteLine(v1);
            Console.WriteLine(v2);
            Console.WriteLine(v1 + v2);

            //Console.ReadKey();
        }
    }
}