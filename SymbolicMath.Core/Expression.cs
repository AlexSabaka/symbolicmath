using SymbolicMath.Symbolic.Operators;
using SymbolicMath.Symbolic.Operators.Scalar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public abstract class Expression : IComparable<Expression>, ICloneable, IEquatable<Expression>
    {
        public virtual double Process() { return 0; }
        public virtual Expression Simplify() { return this; }
        public virtual Expression Derivate(Variable dv) { return this; }

        public virtual Expression Derivate(Variable dv, int times)
        {
            Expression result = this;
            while (times-- > 0) result = result.Derivate(dv).Simplify();
            return result;
        }

        public virtual Expression Replace(Variable v, Expression expr)
        {
            return this;
        }

        public abstract bool IsDependOf(Variable var);

        public static Expression operator +(Expression a)
        {
            return new Pos(a);
        }
        public static Expression operator -(Expression a)
        {
            return new Neg(a);
        }

        public static Expression operator +(Expression a, Expression b)
        {
            return new Add(a, b);
        }
        public static Expression operator -(Expression a, Expression b)
        {
            return new Sub(a, b);
        }
        public static Expression operator *(Expression a, Expression b)
        {
            return new Mul(a, b);
        }
        public static Expression operator /(Expression a, Expression b)
        {
            return new Div(a, b);
        }
        public static Expression operator ^(Expression a, Expression b)
        {
            return new Pow(a, b);
        }

        public static Expression operator +(Expression a, double b)
        {
            return new Add(a, new Constant(b));
        }
        public static Expression operator -(Expression a, double b)
        {
            return new Sub(a, new Constant(b));
        }
        public static Expression operator *(Expression a, double b)
        {
            return new Mul(a, new Constant(b));
        }
        public static Expression operator /(Expression a, double b)
        {
            return new Div(a, new Constant(b));
        }
        public static Expression operator ^(Expression a, double b)
        {
            return new Pow(a, new Constant(b));
        }

        public static Expression operator +(double a, Expression b)
        {
            return new Add(new Constant(a), b);
        }
        public static Expression operator -(double a, Expression b)
        {
            return new Sub(new Constant(a), b);
        }
        public static Expression operator *(double a, Expression b)
        {
            return new Mul(new Constant(a), b);
        }
        public static Expression operator /(double a, Expression b)
        {
            return new Div(new Constant(a), b);
        }
        public static Expression operator ^(double a, Expression b)
        {
            return new Pow(new Constant(a), b);
        }

        public static bool operator ==(Expression a, Expression b)
        {
            if (object.Equals(a, null) && object.Equals(b, null)) return true;
            else if ((object.Equals(a, null) && !object.Equals(b, null)) || 
                     (!object.Equals(a, null) && object.Equals(b, null))) return false;
            else return a.Equals(b);
        }
        public static bool operator !=(Expression a, Expression b)
        {
            return !(a == b);
        }

        public static bool operator >(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        public static bool operator <(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        public static bool operator <=(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Expression(double val)
        {
            return new Constant(val);
        }

        public int CompareTo(Expression other)
        {
            return this > other ? 1 : -1;
        }

        public override bool Equals(object obj)
        {
            return (obj is Expression) ? Equals((Expression)obj) : false;
        }

        public bool Equals(Expression obj)
        {
            bool res = obj.GetHashCode() == this.GetHashCode();
            Debug.WriteLine("\t{0} {2} {1}", this, obj, res ? "==" : "!=");
            return res;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
