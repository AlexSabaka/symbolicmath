using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public class Variable : Expression
    {
        public string Name { get; private set; }

        public double? Value { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public Variable(string name)
        {
            this.Name = name;
        }

        public override double Process()
        {
            if (this.Value.HasValue) return (double)this.Value;
            else throw new ArgumentNullException(this.Name);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override Expression Derivate(Variable dv)
        {
            if (dv.Name == this.Name) return Constant.One;
            else return Constant.Zero;
        }

        public override Expression Simplify()
        {
            return this;
        }

        public override bool IsDependOf(Variable var)
        {
            return var.Name == this.Name;
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            if (this == v) return expr;
            else return this;
        }
    }
}
