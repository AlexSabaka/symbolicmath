using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolicMath.Symbolic
{
    public struct Number : IEquatable<Number>, IComparable<Number>, IFormattable
    {
        public static readonly Number Zero = new Number { mant_sign = 1, mantissa = new List<uint> { 0 } };

        //represents only length
        const uint BASE = 10000;

        int mant_sign;
        List<uint> mantissa;
        //int exp_sign;
        //long exponent;

        public Number(params uint[] mantissa)
            : this()
        {
            this.mant_sign = 1;
            this.mantissa = new List<uint>();
            this.mantissa.AddRange(mantissa.Reverse());
        }

        public Number(string s)
            : this()
        {
            Number n = Parse(s);
            this.mant_sign = n.mant_sign;
            this.mantissa = new List<uint>(n.mantissa);
        }

        private static List<uint> AddMantiss(List<uint> mantissa1, List<uint> mantissa2)
        {
            var ma = new List<uint>();
            ma.AddRange(mantissa1);
            var mb = new List<uint>();
            mb.AddRange(mantissa2);

            if (ma.Count != mb.Count)
            {
                if (ma.Count < mb.Count)
                    while (ma.Count != mb.Count)
                        ma.Add(0);
                if (ma.Count > mb.Count)
                    while (ma.Count != mb.Count)
                        mb.Add(0);
            }

            List<uint> res = new List<uint>();
            for (int i = 0; i < ma.Count; ++i)
            {
                long res_i = ma[i] + mb[i];
                uint res_mantissa_i = 0;
                if (res.Count <= i) res.Add(0);
                if (res_i > BASE)
                {
                    res_mantissa_i = (uint)(res_i - BASE);
                    res.Add(1);
                }
                else res_mantissa_i = (uint)res_i;
                res[i] += res_mantissa_i;
            }
            return res;
        }

        private static List<uint> SubMantiss(List<uint> mantissa1, List<uint> mantissa2, out int res_sign)
        {
            var ma = new List<uint>();
            ma.AddRange(mantissa1);
            var mb = new List<uint>();
            mb.AddRange(mantissa2);
            
            if (ma.Count != mb.Count)
            {
                if (ma.Count < mb.Count)
                    while (ma.Count != mb.Count)
                        ma.Add(0);//.Insert(0, 0);
                if (ma.Count > mb.Count)
                    while (ma.Count != mb.Count)
                        mb.Add(0);//.Insert(0, 0);
            }

            if (mb.Last() > ma.Last())
            {
                res_sign = -1;
                var tmp = new List<uint>(ma);
                ma = new List<uint>(mb);
                mb = new List<uint>(tmp);
            }
            else res_sign=1;

            /*
             *       .  
             *      5960
             *     -1070
             *      ----
             *      4890
             */

            List<uint> res = new List<uint>();
            for (int i = 0; i < ma.Count; ++i)
            {
                uint ima = ma[i];
                uint imb = mb[i];
                if (ima < imb)
                {
                    ima += ma[i + 1];
                }
                uint res_i = ima - imb;

                res.Add(res_i);
            }
            return res;
        }

        private static List<uint> Mul(List<uint> mantissa, uint num)
        {
            List<uint> res = new List<uint>();
            for (int i = 0; i < mantissa.Count; ++i)
            {
                if (res.Count <= i) res.Add(0);
                ulong t = mantissa[i] * num;
                if (t > BASE)
                {
                    uint rem = (uint)(t % BASE);
                    uint div = (uint)(t / BASE);
                    res.Add(rem);
                    res.Add(div);
                }
            }
            return res;
        }

        private static List<uint> MulMantiss(List<uint> mantissa1, List<uint> mantissa2)
        {
            /*      
             *          598
             *        x  82
             *         ----
             *         11  
             *         1196
             *     +  4784 
             *        -----
             *        49036
             */
            List<uint> res = new List<uint>();
            for (int i = 0; i < mantissa2.Count; ++i)
            {
                List<uint> im = Mul(mantissa1, mantissa2[i]);
            }
            return mantissa1;
        }

        public static Number operator +(Number a, Number b)
        {
            if (a.mant_sign == b.mant_sign)
            {
                return new Number { mant_sign = a.mant_sign, mantissa = AddMantiss(a.mantissa, b.mantissa) };
            }
            else
            {
                if (a.mant_sign < 0)
                {
                    var n = new Number();
                    n.mantissa = SubMantiss(b.mantissa, a.mantissa, out n.mant_sign);
                    return n;
                }
                else if (b.mant_sign < 0)
                {
                    var n = new Number();
                    n.mantissa = SubMantiss(a.mantissa, b.mantissa, out n.mant_sign);
                    return n;
                }
            }
            throw new ArgumentException();
        }

        public static Number operator -(Number a, Number b)
        {
            if (a.mant_sign == 1 && b.mant_sign == 1)
            {
                var n = new Number();
                n.mantissa = SubMantiss(a.mantissa, b.mantissa, out n.mant_sign);
                return n;
            }
            else if (a.mant_sign == -1 && b.mant_sign == 1)
            {
                var n = new Number();
                n.mantissa = AddMantiss(a.mantissa, b.mantissa);
                n.mant_sign = -1;
                return n;
            }
            else if (a.mant_sign == 1 && b.mant_sign == -1)
            {
                var n = new Number();
                n.mantissa = AddMantiss(a.mantissa, b.mantissa);
                n.mant_sign = 1;
                return n;
            }
            else if (a.mant_sign == -1 && b.mant_sign == -1)
            {
                var n = new Number();
                n.mantissa = SubMantiss(b.mantissa, a.mantissa, out n.mant_sign);
                return n;
            }
            else throw new ArgumentException();
        }

        public static Number operator -(Number a)
        {
            return new Number { mant_sign = -a.mant_sign, mantissa = a.mantissa };
        }

        public static Number operator *(Number a, Number b)
        {
            throw new NotImplementedException();
        }

        public static Number operator /(Number a, Number b)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Number other)
        {
            if (other.mant_sign != mant_sign) return false;
            if (other.mantissa.Count != mantissa.Count) return false;
            return mantissa == other.mantissa;
        }

        public int CompareTo(Number other)
        {
            throw new NotImplementedException();
        }

        public static Number Parse(string s)
        {
            int sign = 0;
            if (s[0] == '-')
            {
                sign = -1;
                s = s.Substring(1);
            }
            else if (s[0] == '+')
            {
                sign = 1;
                s = s.Substring(1);
            }
            else if (char.IsDigit(s[0])) sign = 1;
            else throw new FormatException();

            List<uint> mant = new List<uint>();
            s = string.Join("", s.Reverse());
            int l = BASE.ToString().Length - 1;
            for (int i = 0; i < s.Length; i += l)
            {
                string p = s.Substring(i, s.Length - i - l < 0 ? s.Length - i : l);
                p = string.Join("", p.Reverse());
                mant.Add(uint.Parse(p));
            }

            return new Number { mant_sign = sign, mantissa = mant };
        }

        public static bool TryParse(string s, out Number result)
        {
            int sign = 0;
            if (s[0] == '-')
            {
                sign = -1;
                s = s.Substring(1);
            }
            else if (s[0] == '+')
            {
                sign = 1;
                s = s.Substring(1);
            }
            else if (char.IsDigit(s[0])) sign = 1;
            else
            {
                result = Zero;
                return false;
            }

            List<uint> mant = new List<uint>();
            s = string.Join("", s.Reverse());
            int l = BASE.ToString().Length - 1;
            for (int i = 0; i < s.Length; i += l)
            {
                string p = s.Substring(i, l);
                p = string.Join("", p.Reverse());
                uint pn = 0;
                if (!uint.TryParse(p, out pn))
                {
                    result = Zero;
                    return false;
                }
                mant.Add(pn);
            }

            result = new Number { mant_sign = sign, mantissa = mant };
            return true;
        }

        public override string ToString()
        {
            return ToString("", System.Globalization.CultureInfo.InvariantCulture);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("", formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string res = string.Join("", mantissa.Reverse<uint>().Select((n, i) => 
                {
                    string ns =  n.ToString(format, formatProvider);
                    return i != 0 ? ns.PadLeft(BASE.ToString().Length - 1, '0') : ns;
                }));
            res = res.TrimStart('0');
            return (mant_sign == -1 ? "-" : "") + res;
        }
    }
}
