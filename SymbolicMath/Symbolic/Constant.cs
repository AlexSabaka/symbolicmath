using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public class Constant : Expression
    {
        public static readonly Constant Zero = new Constant(0.0);
        public static readonly Constant One = new Constant(1.0);
        public static readonly Constant Two = new Constant(2.0);
        public static readonly Constant MinusOne = new Constant(-1.0);
        public static readonly Constant Inf = new Constant(double.PositiveInfinity, "Infinity");
        public static readonly Constant NaN = new Constant(double.NaN, "NaN");
        public static readonly Constant E = new Constant(Math.E, "E");
        public static readonly Constant Pi = new Constant(Math.PI, "Pi");

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Constant) ? Equals((Constant)obj) : false;
        }

        public bool Equals(Constant obj)
        {
            double dsq = Math.Pow(Math.Abs(this.Value - obj.Value), 2);
#warning It may occures to computing error
            return dsq < Math.Pow(10, -30);
        }

        public static bool operator ==(Constant a, Constant b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Constant a, Constant b)
        {
            return !(a == b);
        }
        public static bool operator >(Constant a, Constant b)
        {
            return a.Value > b.Value;
        }
        public static bool operator <(Constant a, Constant b)
        {
            return a.Value < b.Value;
        }
        public static bool operator >=(Constant a, Constant b)
        {
            return a.Value >= b.Value;
        }
        public static bool operator <=(Constant a, Constant b)
        {
            return a.Value <= b.Value;
        }

        public string Name { get; private set; }
        public bool IsNamedConst { get { return Name != null; } }

        public Constant(double value)
        {
            this.Value = value;
        }

        public Constant(double value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public double Value { get; private set; }

        public override string ToString()
        {
            return IsNamedConst ? Name : Value.ToString();
        }

        public static implicit operator Constant(double value)
        {
            return new Constant(value);
        }

        public override double Process()
        {
            return this.Value;
        }

        public override Expression Derivate(Variable dv)
        {
            return Constant.Zero;
        }

        public override Expression Simplify()
        {
            return this;
        }

        public override bool IsDependOf(Variable var)
        {
            return false;
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return this;
        }
    }
}
