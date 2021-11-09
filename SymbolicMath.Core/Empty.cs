using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public class Empty : Expression
    {
        public override double Process()
        {
            throw new NotImplementedException();
        }

        public override Expression Simplify()
        {
            return this;
        }

        public override Expression Derivate(Variable dv)
        {
            return this;
        }

        public override bool IsDependOf(Variable var)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            return this;
        }
    }
}
