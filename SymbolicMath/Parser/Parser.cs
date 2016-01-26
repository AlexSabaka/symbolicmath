using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Parser
{
    public class Parser
    {
        public string RemoveComments(string s)
        {
            string res = "";
            bool comment = false;
            for (int i = 0; i < s.Length; ++i)
            {
                if (i + 1 >= s.Length)
                {
                    if (!comment) res += s[i];
                    break;
                }
                if (s[i] == '/' && (s[i + 1] == '/' || s[i + 1] == '*')) comment = true;
                if (comment && s[i] == '\n') comment = false;
                if (comment && (s[i] == '*' && s[i + 1] == '/'))
                {
                    comment = false;
                    i += 2;
                }

                if (!comment) res += s[i];
            }
            return res;
        }

        public IEnumerable<string> Parse(string s)
        {
            s = RemoveComments(s);
            char[] delims = new[] { '+', '.', ',', ';', '+', '-', '*', '/', '\'', '^', '!', '(', ')', '[', ']' };
            char[] white = new[] { ' ', '\t', '\r', '\n' };
            string acc = "";
            List<string> res = new List<string>();
            int line = 0;
            int pos = 0;
            bool isNum = false;
            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] == '\n')
                {
                    line++;
                    pos = 0;
                }
                if (white.Contains(s[i]) || delims.Contains(s[i]))
                {
                    if (s[i] == '.')
                    {
                        if (isNum && acc.Contains('.')) throw new ParserException("Number expected.", line, pos);
                        if (!isNum && i + 1 < s.Length && char.IsNumber(s[i + 1])) isNum = true;
                    }
                    if ((s[i] == '+' || s[i] == '-'))
                    {
                        if (i + 1 > s.Length) throw new UnexpectedEofException(line, pos);
                        else if (char.IsNumber(s[i + 1]))
                        {
                            isNum = true;
                        }
                    }
                    if (char.IsNumber(s[i])) isNum = true;

                    goto end;
                otherwise: ;
                    if (!string.IsNullOrEmpty(acc)) res.Add(acc);
                    if (!white.Contains(s[i])) res.Add(s[i].ToString());
                    acc = "";
                    continue;
                end: ;
                }
                acc += s[i];
                pos++;
            }
            if (!string.IsNullOrEmpty(acc)) res.Add(acc);
            return res;
        }
    }

    public class Tokenizer
    {
    }

    public class Lexer
    {
    }
}