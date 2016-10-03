using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleApplication2
{

    public class Number:IComparable<Number>
    {
        private List<byte> _digits = new List<byte>();
        private bool _positive = true;

        public virtual int Digits
        {
            get { return _digits.Count; }
        }

        public Number()
        {
            _digits.Add(0);
        }

        public Number(int value)
        {
            if (value == 0)
            {
                _digits.Add(0);
                return;
            }

            if(value<0)
            {
                value *= -1;
                _positive = false;
            }

            while(value>0)
            {
                _digits.Add((byte)(value%10));
                value /= 10;
            }
            
        }

        public Number(Number other)
        {
            _digits = new List<byte>(other._digits);
        }

        public override string ToString()
        {
            string str = string.Join("", _digits);
            str=new string (str.Reverse().ToArray());
            if (!_positive)
                str = "-" + str;
            return str;
        }

        public virtual byte this[int index]
        {
            get { return _digits[_digits.Count - index - 1]; }
        }

        public static Number Add(Number left, Number right)
        {
            Number result;
            Number tmp;
            byte p = 0;

            if (left._digits.Count > right._digits.Count)
            {
                result = new Number(left);
                tmp = right;
            }
            else
            {
                result = new Number(right);
                tmp = left;
            }

            for (int i=0; i< result._digits.Count; i++)
            {
                if (tmp._digits.Count > i)
                    result._digits[i] += (byte)(tmp._digits[i] + p);
                else
                    result._digits[i] += p;

                p = (byte)(result._digits[i] / 10);
                result._digits[i] %= 10;
            }

            if (p > 0)
                result._digits.Add(1);

            return result;


        }

        public static Number operator + (Number left, Number right)
        {
            return Add(left,right);
        }

        public static bool operator <(Number left, Number right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator >(Number left, Number right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool operator <=(Number left, Number right)
        {
            return left.CompareTo(right) <=0;
        }

        public static bool operator >=(Number left, Number right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(Number left, Number right)
        {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Number left, Number right)
        {
            return !(left==right);
        }

        public int CompareTo(Number other)
        {
            if (_positive && other._positive == false)
                return 1;
            if (!_positive && other._positive == true)
                return -1;
            if (_positive && other._positive)
            {
                string str = this.ToString();
                string otherStr = other.ToString();

                if (str.Length < otherStr.Length)
                    return -1;
                if (str.Length > otherStr.Length)
                    return 1;
                return string.Compare(str, otherStr);
            }
            else
            {
                string str = this.ToString();
                string otherStr = other.ToString();

                if (str.Length < otherStr.Length)
                    return 1;
                if (str.Length > otherStr.Length)
                    return -1;
                return -1*string.Compare(str, otherStr);
            }
        }

        public static explicit operator long(Number n)
        {
            long l;
            long.TryParse(n.ToString(), out l);

            return l;
        }

        public static explicit operator byte(Number n)
        {
            byte b = byte.Parse(n.ToString());

            return b;
        }

    }

    public class NumberDerived:Number
    {

    }

    public class Calculator
    {
        public static decimal CalculateIf(string op, decimal left, decimal right)
        {
            if (op == "+")
                return Add(left, right);
            if (op == "-")
                return Subtract(left, right);
            if (op == "*")
                return Multiply(left, right);
            if (op == "/")
                return Divide(left, right);

            return 0;
        }

        public static decimal CalculateSwitch(string op, decimal left, decimal right)
        {
            switch (op)
            {
                case "+":
                    return Add(left, right);
                case "-":
                    return Subtract(left, right);
                case "*":
                    return Multiply(left, right);
                case "/":
                    return Divide(left, right);
                default:
                    return 0;
            }
        }

        public delegate decimal Arithmetic(decimal left, decimal right);

        //Arithmetic o;

        public decimal CalculateDelegate(string op, decimal left, decimal right)
        {
            Arithmetic o;

            switch (op)
            {
                case "+":
                    o= Add;
                    break;
                case "-":
                    o= Subtract;
                    break;
                case "*":
                    o= Multiply;
                    break;
                case "/":
                    o= Divide;
                    break;
                default:
                    return 0;
            }

            return o(left, right);
        }

        public decimal CalculateAnonDelegate(string op, decimal left, decimal right)
        {
            Func<decimal,decimal,decimal> o;

            switch (op)
            {
                case "+":
                    o = Add;//o=((x,y)=>x+y;
                    break;
                case "-":
                    o = Subtract;
                    break;
                case "*":
                    o = Multiply;
                    break;
                case "/":
                    o = Divide;
                    break;
                default:
                    return 0;
            }

            return o(left, right);
        }

        static Dictionary<string, Func<decimal, decimal, decimal>> operations = new Dictionary<string, Func<decimal, decimal, decimal>>()
        {
            { "+",(x,y)=>x+y},
            { "-",(x,y)=>x-y},
            { "*",(x,y)=>x*y},
            { "/",(x,y)=>x/y}
        };

        public static decimal CalculateDictionary(string op, decimal left, decimal right)
        {
            if (!operations.ContainsKey(op)) return 0;

            return operations[op](left, right);
        }
        private static decimal Add(decimal left, decimal right)
        {
            return left + right;
        }

        private static decimal Subtract(decimal left, decimal right)
        {
            return left - right;
        }

        private static decimal Multiply(decimal left, decimal right)
        {
            return left * right;
        }

        private static decimal Divide(decimal left, decimal right)
        {
            return left / right;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Number n = new Number(-8);
            Number m = new Number(1);
            Number sum = Number.Add(n,m);

            Console.WriteLine(sum);
            */


            
            
        }
    }
}
