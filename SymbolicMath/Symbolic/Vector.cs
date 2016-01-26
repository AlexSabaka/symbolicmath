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
        public static Vector Zero(int dimentions)
        {
            return new Vector();
        }

        public static Vector One(int dimentions)
        {
            return new Vector();
        }

        public static Vector Unit(int dimentions, int i)
        {
            return new Vector();
        }

        public Expression[] Components { get; set; }

        public Vector()
            : this(0) { }
        public Vector(Expression x, Expression y)
            : this(new[] { x, y }) { }
        public Vector(Expression x, Expression y, Expression z)
            : this(new[] { x, y, z }) { }
        public Vector(params Expression[] components)
        {
            this.Components = components;
        }

        public override Expression Derivate(Variable dv)
        {
            throw new NotImplementedException();
        }

        public override bool IsDependOf(Variable var)
        {
            throw new NotImplementedException();
        }

        public new double[] Process()
        {
            throw new NotImplementedException();
        }

        public new Vector Replace(Variable v, Expression expr)
        {
            throw new NotImplementedException();
        }

        public new Vector Simplify()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "{" + string.Join(", ", (object[])this.Components) + "}";
        }

        public static Vector operator +(Vector a)
        {
            throw new NotImplementedException();
        }
        public static Vector operator -(Vector a)
        {
            throw new NotImplementedException();
        }

        public static Vector operator +(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
        public static Vector operator -(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
        public static Vector operator *(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
                
        public static Vector operator *(Vector a, double b)
        {
            throw new NotImplementedException();
        }
        public static Vector operator /(Vector a, double b)
        {
            throw new NotImplementedException();
        }

        public static Vector operator *(double a, Vector b)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Vector a, Vector b)
        {
            throw new NotImplementedException();
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

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
