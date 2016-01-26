using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic.Operators
{
    public class Operator
    {
        public static readonly Operator Add = new Operator(OperatorType.Addition, "+", false, true, true, true);
        public static readonly Operator Sub = new Operator(OperatorType.Substraction, "-", false, true, true, true);
        public static readonly Operator Mul = new Operator(OperatorType.Multiply, "*", false, true, true, true);
        public static readonly Operator Div = new Operator(OperatorType.Divide, "/", false, true, false, false);
        public static readonly Operator Pow = new Operator(OperatorType.Power, "^", false, true, false, true);
        public static readonly Operator Neg = new Operator(OperatorType.Negation, "-", true, true, true, true);
        public static readonly Operator Pos = new Operator(OperatorType.Positive, "+", true, true, true, true);
        public static readonly Operator Factorial = new Operator(OperatorType.Factorial, "!", true, true, false, false);
        public static readonly Operator Derivative = new Operator(OperatorType.Derivative, "D", true, true, true, true);
        public static readonly Operator Cross = new Operator(OperatorType.Cross, "×", false, false, true, false);
        public static readonly Operator Dot = new Operator(OperatorType.Dot, "·", false, false, true, false);
        public static readonly Operator Gradient = new Operator(OperatorType.Gradient, "∇", true, true, false, false);
        public static readonly Operator Curl = new Operator(OperatorType.Curl, "∇×", true, false, true, false);
        public static readonly Operator Divergence = new Operator(OperatorType.Divergence, "∇·", true, false, true, false);
        public static readonly Operator Laplacian = new Operator(OperatorType.Laplacian, "∆", true, true, true, false);

        public OperatorType Type { get; set; }

        public string StringRepresentation { get; set; }

        public bool IsUnary { get; private set; }

        public bool IsRightAssigned
        {
            get
            {
                return Type == OperatorType.Factorial; // || Type == OperatorType.Derivative;
            }
        }

        public bool IsVectorOperator { get; private set; }

        public bool IsMatrixOperator { get; private set; }

        public bool IsScalarOperator { get; private set; }

        public Operator(OperatorType type)
        {
            this.Type = type;
        }

        public Operator(OperatorType type, string representation, bool unary, bool scalar, bool vector, bool matrix)
            : this(type)
        {
            this.StringRepresentation = representation;
            this.IsUnary = unary;
            this.IsScalarOperator = scalar;
            this.IsVectorOperator = vector;
            this.IsMatrixOperator = matrix;
        }

        public double Apply(double left, double right)
        {
            switch (this.Type)
            {
                case OperatorType.Addition: return left + right;
                case OperatorType.Substraction: return left - right;
                case OperatorType.Multiply: return left * right;
                case OperatorType.Divide: return left / right;
                case OperatorType.Power: return Math.Pow(left, right);
                default: throw new InvalidOperationException(this.Type.ToString());
            }
        }

        public double Apply(double left)
        {
            if (!IsUnary) throw new InvalidOperationException("Sassay lalka!");
            switch (this.Type)
            {
                case OperatorType.Negation: return -left;
                case OperatorType.Positive: return left;
                case OperatorType.Factorial: throw new NotImplementedException();
                case OperatorType.Derivative: throw new NotImplementedException();
                default: throw new InvalidOperationException(this.Type.ToString());
            }
        }

        public string CreateFormat()
        {
            if (!IsUnary) return "({0} " + StringRepresentation + " {1})";
            else
            {
                if (IsRightAssigned) return "({0})" + StringRepresentation;
                else return StringRepresentation + "({0})";
            }
        }

        public override string ToString()
        {
            return Type.ToString() + " [" + StringRepresentation + "]";
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
