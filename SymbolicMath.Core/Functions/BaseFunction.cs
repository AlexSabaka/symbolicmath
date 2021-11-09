using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Functions
{
    public class FunctionDefinition : BaseFunction
    {
        public new string Name { get; private set; }
        public Expression Body { get; set; }

        public FunctionDefinition(string name, Expression body, params Expression[] args)
            : base(args)
        {
            this.Name = name;
            this.Args = args;
            this.Body = body;
        }

        public override double Process()
        {
            return 0;
        }

        public override Expression Simplify()
        {
            return new FunctionDefinition(this.Name, this.Body.Simplify(), this.Args);
        }

        public override Expression Derivate(Variable dv)
        {
            return new FunctionDefinition(this.Name, this.Body.Derivate(dv), this.Args);
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", this.Name, string.Join(", ", this.Args.Select(subj => subj.ToString())));
        }
    }

    public abstract class BaseFunction : Expression
    {
        public string Name { get { return this.GetType().Name; } }
        public Expression[] Args { get; set; }

        public BaseFunction(params Expression[] args)
        {
            this.Args = args;
        }

        public override int GetHashCode()
        {
            int argsHash = Args.Sum(a => a.GetHashCode() & 0x0000FFFF);
            return Name.GetHashCode() ^ argsHash;
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", this.Name, string.Join(", ", this.Args.Select(subj => subj.ToString())));
        }

        public abstract override double Process();

        public abstract override Expression Simplify();

        public abstract override Expression Derivate(Variable dv);

        public override bool IsDependOf(Variable var)
        {
            foreach (var a in this.Args) if (a.IsDependOf(var)) return true;
            return false;
        }

        public override Expression Replace(Variable v, Expression expr)
        {
            BaseFunction bf = (BaseFunction)this.MemberwiseClone();
            for (int i = 0; i < bf.Args.Length; ++i) bf.Args[i] = bf.Args[i].Replace(v, expr);
            return bf;
        }
    }
}
