using SymbolicMath.Symbolic.Operators.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public class Vector : Expression
    {
        public static Vector Zero(int dimensions) => Enumerable.Repeat(Constant.Zero, dimensions).ToArray();

        public static Vector One(int dimensions) => Enumerable.Repeat(Constant.One, dimensions).ToArray();

        public static Vector Unit(int dimensions, int i)
        {
            var zeroes = new Expression[dimensions];
            zeroes[i] = Constant.One;
            return zeroes;
        }

        public Expression[] Components { get; set; }

        public Vector(int dimensions)
        {
            Components = new Expression[dimensions];
        }
        public Vector()
            : this(0) { }
        public Vector(Expression x, Expression y)
            : this(new[] { x, y }) { }
        public Vector(Expression x, Expression y, Expression z)
            : this(new[] { x, y, z }) { }
        public Vector(params Expression[] components)
        {
            Components = components;
        }

        public override Expression Derivate(Variable dv) => (Vector)Components.Select(x => x.Derivate(dv)).ToArray();

        public override bool IsDependOf(Variable var) => Components.Any(x => x.IsDependOf(var));

        public new double[] Process() => Components.Select(x => x.Process()).ToArray();

        public new Vector Replace(Variable v, Expression expr) => Components.Select(x => x.Replace(v, expr)).ToArray();

        public new Vector Simplify() => Components.Select(x => x.Simplify()).ToArray();

        public override string ToString() => "{" + string.Join<Expression>(", ", Components) + "}";

        public static implicit operator Expression[](Vector vector) => vector?.Components;

        public static implicit operator Vector(Expression[] components) => new Vector(components);

        public static Vector operator +(Vector a) => a.Components.Select(x => +x).ToArray();
        public static Vector operator -(Vector a) => a.Components.Select(x => -x).ToArray();

        public static Vector operator +(Vector a, Vector b) => a.Components.Join(b.Components, x => x, x => x, (ax, bx) => ax + bx).ToArray();
        public static Vector operator -(Vector a, Vector b) => a.Components.Join(b.Components, x => x, x => x, (ax, bx) => ax - bx).ToArray();
        public static Vector operator *(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
                
        public static Vector operator *(Vector a, double b) => a.Components.Select(x => x * b).ToArray();
        public static Vector operator /(Vector a, double b) => a.Components.Select(x => x / b).ToArray();

        public static Vector operator *(double a, Vector b) => b.Components.Select(x => a * x).ToArray();

        public static bool operator ==(Vector a, Vector b) => a.Components.Join(b.Components, x => x, x => x, (ax, bx) => ax == bx).All(x => x);
        public static bool operator !=(Vector a, Vector b) => !(a == b);

        public static bool operator >(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
        public static bool operator <(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
        public static bool operator <=(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
    }
}
