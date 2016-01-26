using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymbolicMath.Parser
{
    public class ParserException : Exception
    {
        public ParserException(string msg, int line, int pos)
            : base(string.Format("[{0}:{1}] {2}", line, pos, msg))
        { }

        public ParserException(string msg)
            : base(msg)
        { }
    }

    public class UnexpectedCharacterException : ParserException
    {
        public UnexpectedCharacterException(char c, int line, int pos)
            : base(string.Format("Unexpected character '{0}'.", c), line, pos)
        { }
    }

    public class UnexpectedEofException : ParserException
    {
        public UnexpectedEofException(int line, int pos) :
            base("Unexpected end of file.", line, pos) { }
    }
}
