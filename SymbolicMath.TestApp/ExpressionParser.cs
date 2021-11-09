
using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using SymbolicMath.Symbolic;
using SymbolicMath.Symbolic.Functions;

public class X
    {
        static readonly Dictionary<string, Expression> _variables = new Dictionary<string, Expression>();
        static readonly Dictionary<string, Expression> _functions = new Dictionary<string, Expression>();

        enum opType
        {
            add, sub, mul, div, pow
        }
        static Expression Assign(Expression var, Expression expr)
        {
            if (var is Variable v)
            {
                if (_variables.ContainsKey(v.Name)) _variables[v.Name] = expr;
                else _variables.Add(v.Name, expr);
            }
            else
            {
                throw new NotSupportedException();
            }

            return expr;
        }
        static Expression AssignFunction((string name, string[] args) funcDef, Expression expr)
        {
            var func = new FunctionDefinition(funcDef.name, expr, funcDef.args.Select(x => new Variable(x)).ToArray());
            if (_functions.ContainsKey(funcDef.name))
            {
                _functions[funcDef.name] = func;
            }
            else
            {
                _functions.Add(funcDef.name, func);
            }

            return func;
        }
        static Expression Variable(string name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];
            else
                return new Variable(name);
        }
        static Parser<opType> Operator(string op, opType type)
        {
            return Parse.String(op).Token().Return(type);
        }

        static Expression binaryApply(opType op, Expression a, Expression b)
            => op switch {
                opType.add => a + b,
                opType.sub => a - b,
                opType.mul => a * b,
                opType.div => a / b,
                opType.pow => a ^ b,
                _ => throw new ParseException(),
            };

        static Expression function(string name, IEnumerable<Expression> args)
        {
            if (_functions.ContainsKey(name))
            {
                var body = _functions[name];
                return new FunctionDefinition(name, body, args.ToArray());
            }

            var methodInfo = typeof(SMath).GetMethod(name);
            if (methodInfo == null)
                throw new ParseException($"Function '{name}' does not exist.");

            var argsCount = methodInfo.GetParameters().Length;

            return methodInfo.Invoke(null, args.Take(argsCount).Cast<object>().ToArray()) as Expression;
        }

        static readonly Parser<opType> add = Operator("+", opType.add);
        static readonly Parser<opType> sub = Operator("-", opType.sub);
        static readonly Parser<opType> div = Operator("/", opType.div);
        static readonly Parser<opType> mul = Operator("*", opType.mul);
        static readonly Parser<opType> pow = Operator("^", opType.pow);
        static readonly Parser<Expression> constant = Parse.Decimal.Select(x => new Constant(double.Parse(x)));
        static readonly Parser<string> identifierString = Parse.Identifier(
                                                                Parse.Letter.Or(Parse.Char('_')),
                                                                Parse.LetterOrDigit.Or(Parse.Char('_')))
                                                            .Token();
        static readonly Parser<(string, string[])> funcIdentifierString = (from name in identifierString
                                                                from lparen in Parse.Char('[')
                                                                from args in Parse.Ref(() => identifierString).DelimitedBy(Parse.Char(',').Token().Optional())
                                                                from rparen in Parse.Char(']')
                                                                select (name, args.ToArray()));
        static readonly Parser<Expression> identifier = identifierString
                                                            .Select(Variable)
                                                            .Or(constant);
        static readonly Parser<Expression> funcCall = (from func in identifierString
                                                        from lparen in Parse.Char('[')
                                                        from args in Parse.Ref(() => expr).DelimitedBy(Parse.Char(',').Token().Optional())
                                                        from rparen in Parse.Char(']')
                                                        select function(func, args))
                                                    .Or(identifier);
        static readonly Parser<Expression> derivate = (from fact in Parse.Ref(() => expr)
                                                        from d in Parse.Char('\'')
                                                        from dv in identifierString
                                                        select fact.Derivate(new Variable(dv)))
                                                       .XOr(identifier);
        static readonly Parser<Expression> factor = (from lparen in Parse.Char('(')
                                                        from ex in Parse.Ref(() => expr)
                                                        from rparen in Parse.Char(')')
                                                        select ex)
                                                    .XOr(funcCall);
        static readonly Parser<Expression> operand = ((from sign in Parse.Char('-')
                                                        from fact in factor
                                                        select -fact)
                                                    .XOr(factor))
                                                    .Token();
        static readonly Parser<Expression> innerTerm = Parse.ChainOperator(pow, operand, binaryApply);
        static readonly Parser<Expression> term = Parse.ChainOperator(mul.Or(div), innerTerm, binaryApply);
        static readonly Parser<Expression> expr = Parse.ChainOperator(add.Or(sub), term, binaryApply);
        static readonly Parser<Expression> funcAssign = (from funcDef in funcIdentifierString
                                                    from eq in Parse.Char('=').Token()
                                                    from ex in expr
                                                    select AssignFunction(funcDef, ex));
        static readonly Parser<Expression> assign = (from variable in identifier
                                                    from eq in Parse.Char('=').Token()
                                                    from ex in expr
                                                    select Assign(variable, ex))
                                                    .Or(funcAssign);

        static void ParseAndPrintExpression(string s)
        {
            try
            {
                var r = assign.Parse(s);
                Console.WriteLine("\t{0}", r);
            }
            catch (ParseException ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public static void Main2(string[] args)
        {
            while (true)
            {
                Console.Write(">> ");
                var s = Console.ReadLine();
                Expression r = new Empty();
                switch (s)
                {
                    case "help":
                        break;
                    case "func":
                        Console.WriteLine("\t" + string.Join("\n\t", _functions.Select(x => $"{x.Value} = {(x.Value as FunctionDefinition).Body}")));
                        break;
                    case "vars":
                        Console.WriteLine("\t" + string.Join("\n\t", _variables.Select(x => $"{x.Key} = {x.Value}")));
                        break;
                    case "cv":
                        Console.WriteLine("\t" + string.Join("\n\t", _variables.Select(x => $"{x.Key} = {x.Value.Process()}")));
                        break;
                    default:
                        ParseAndPrintExpression(s);
                        break;
                }
            }
        }
    }
