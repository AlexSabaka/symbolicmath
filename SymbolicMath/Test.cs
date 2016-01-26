using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymbolicMath.Symbolic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SymbolicMath
{
    [TestClass]
    public class TestMe
    {
        [TestMethod]
        public void TestParser()
        {
            var p = new Parser.Parser();
            Assert.AreEqual(p.RemoveComments("lol;//Comment"), "lol;");
            Assert.AreEqual(p.RemoveComments("lol;//Comment\nlol;//Comment 2\nlol/*Alex Sabala*/;"), "lol;\nlol;\nlol;");
            Assert.AreEqual(p.Parse("lol;\nlol;").Count(), 4);
        }

        [TestMethod]
        public void TestBigNumbers()
        {
            Number a = Number.Parse("555555555555555555555555555555555");
            Number b = Number.Parse("-111111111111111111111111111111111");
            Number c = a + b;

            Assert.AreEqual(c.ToString(), "444444444444444444444444444444444");
        }

        [TestMethod]
        public void TestForSimplification()
        {
            Variable x = new Variable("x");

            Expression s1 = SMath.Exp(SMath.Ln(x));
            Assert.AreEqual(s1.Simplify(), x);

            Expression s2 = SMath.Ln(SMath.Exp(x));
            Assert.AreEqual(s2.Simplify(), x);

            Expression s3 = SMath.Sqrt((SMath.Exp(SMath.Ln(x)) - x) ^ 10);
            Assert.AreEqual(s3.Simplify(), Constant.Zero);

            Expression s4 = (x ^ 6) / (x ^ 4);
            Assert.AreEqual(s4.Simplify(), x ^ 2);

            Expression s5 = (x ^ 6) * (x ^ 9);
            Assert.AreEqual(s5.Simplify(), x ^ 15);

            Expression s6 = (Constant.One ^ 6) ^ 2;
            Assert.AreEqual(s6.Simplify(), Constant.One);
        }

        [TestMethod]
        public void TestForComparation()
        {
            Variable x = new Variable("x");

            var c1 = x + x;
            var c2 = x + x;
            Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
        }

        [TestMethod]
        public void TestForDerivation()
        {
            Variable x = new Variable("x");
            Variable y = new Variable("y");
            Expression e = SMath.Exp(x + y);

            Expression dx = e.Derivate(x).Simplify();
            Assert.AreEqual(dx, e);

            Expression dy = e.Derivate(y).Simplify();
            Assert.AreEqual(dy, e);

            Expression dxy = e.Derivate(x).Derivate(y).Simplify();
            Assert.AreEqual(dxy, e);

            Expression xdy = x.Derivate(y).Simplify();
            Assert.AreEqual(xdy, Constant.Zero);

            Expression ydx = y.Derivate(x).Simplify();
            Assert.AreEqual(ydx, Constant.Zero);

            Expression d1 = (x * y).Derivate(x).Simplify();
            Assert.AreEqual(d1, y);

            Expression d2 = (x * y).Derivate(y).Simplify();
            Assert.AreEqual(d2, x);

            Expression d3 = (x + y).Derivate(x).Simplify();
            Assert.AreEqual(d3, Constant.One);

            Expression d4 = (x + y).Derivate(y).Simplify();
            Assert.AreEqual(d4, Constant.One);

            Expression d5 = (1 / x).Derivate(x).Simplify();
            Assert.AreEqual(d5, -(1 / x ^ 2));
        }
    }
}
