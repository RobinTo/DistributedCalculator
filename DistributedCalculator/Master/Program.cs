using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Master
{
    class Program
    {
        static void Main(string[] args)
        {
            string defaultExpression = "(cos(2)+2!*(4^2)/((sin(1))^2)+(4mod3)-12)";
            bool running = true;
            while (running)
            {
                Console.WriteLine("Enter an expression to calculate, click enter to see sample expression, or exit to exit. If an expression doesn't work, write more parantheses.");
                Console.WriteLine("");
                string expression = Console.ReadLine();
                if (expression.ToLower() == "exit")
                    break;
                else if (expression == string.Empty)
                    expression = defaultExpression;
                Console.WriteLine("Calculating: " + expression);
                expression = expression.Replace(".", ",");
                expression = expression.Replace(" ", String.Empty);
                while (expression.Contains(")("))
                {
                    expression = expression.Insert(expression.IndexOf(")(") + 1, "*");
                }
                expression = expression.ToLower();
                expression = expression.Insert(0, "(");
                expression = expression.Insert(expression.Length, ")");
                List<string> paranthesis = new List<string>();

                bool runAgain = true;
                int left = -1;
                while (runAgain)
                {
                    int right = expression.IndexOf(')');

                    if (right > 0 && right < expression.Length)
                    {
                        for (int i = 1; i <= right; i++)
                        {
                            left = expression.IndexOf('(', right - i, i);
                            if (left > 0 && left < expression.Length)
                                break;
                        }
                        if (left == -1)
                            break;
                        paranthesis.Add(expression.Substring(left, right - left + 1));
                        expression = expression.Remove(left, right - left + 1);
                        expression = expression.Insert(left, (char)('a' + (paranthesis.Count - 1)) + "arg");
                    }
                    else
                    {
                        runAgain = false;
                    }
                }

                for (int i = 0; i < paranthesis.Count; i++)
                {
                    string currentExpression = paranthesis[i].Replace("(", String.Empty);
                    currentExpression = currentExpression.Replace(")", String.Empty);

                    paranthesis[i] = CalculateExpression(currentExpression, paranthesis).ToString();
                }

                Console.WriteLine("Solution: " + paranthesis[paranthesis.Count - 1]);
                Console.WriteLine("");
            }
        }

        public static double CalculateExpression(string expression, List<string> paranthesis)
        {
            double partialResult = 0;

            char[] operators = new char[] { '-', '+', '*', '/' };

            while (expression.Contains("arg"))
            {
                char counter = (expression.Substring(expression.IndexOf("arg")-1, 1))[0];
                expression = expression.Replace(counter + "arg", paranthesis[(int)(counter-97)]);
            }

            if (expression[0] == '-')
            {
                expression = expression.Substring(1, expression.Length - 1);
                expression = expression.Insert(0, "neg");
            }
            expression = expression.Replace("*-", "*neg");
            expression = expression.Replace("/-", "/neg");
            expression = expression.Replace("+-", "+neg");
            expression = expression.Replace("--", "-neg");
            expression = expression.Replace("cos-", "cosneg");
            expression = expression.Replace("sin-", "sinneg");
            expression = expression.Replace("mod-", "modneg");
            expression = expression.Replace("^-", "^neg");

            string[] expressionParts = expression.Split(operators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expressionParts.Count(); i++)
            {
                expression = ReplaceFirst(expression, expressionParts[i], "arg" + (char)('a' + i));
            }

            for (int i = 0; i < expressionParts.Count(); i++)
            {
                partialResult = 0;
                try
                {
                    partialResult = double.Parse(expressionParts[i].Replace("neg", "-"));
                }
                catch (Exception e)
                { }
                if (expressionParts[i].Contains("mod"))
                {
                    double first = double.Parse(expressionParts[i].Substring(0, expressionParts[i].IndexOf("mod")).Replace("neg", "-"));
                    double second = double.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("mod") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("mod") + 3)).Replace("neg", "-"));

                    IModOrFactorial proxy = (IModOrFactorial)XmlRpcProxyGen.Create(typeof(IModOrFactorial));
                    Result ret = proxy.Modulus(first, second);
                    partialResult = ret.result;
                }
                if (expressionParts[i].Contains("cos"))
                {
                    double cosNumber = double.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("cos") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("cos") + 3)).Replace("neg", "-"));
                    ITrigonometry proxy = (ITrigonometry)XmlRpcProxyGen.Create(typeof(ITrigonometry));
                    Result ret = proxy.Cosinus(cosNumber);
                    partialResult = ret.result;
                }
                if (expressionParts[i].Contains("sin"))
                {
                    double sinNumber = double.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("sin") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("sin") + 3)).Replace("neg", "-"));
                    ITrigonometry proxy = (ITrigonometry)XmlRpcProxyGen.Create(typeof(ITrigonometry));
                    Result ret = proxy.Sinus(sinNumber);
                    partialResult = ret.result;
                }
                if (expressionParts[i].Contains("^"))
                {
                    double first = double.Parse(expressionParts[i].Substring(0, expressionParts[i].IndexOf("^")).Replace("neg","-"));
                    double second = double.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("^") + 1, expressionParts[i].Length - (expressionParts[i].IndexOf("^") + 1)).Replace("neg", "-"));
                    IPowerOf proxy = (IPowerOf)XmlRpcProxyGen.Create(typeof(IPowerOf));
                    Result ret = proxy.PowerOf(first, second);
                    partialResult = ret.result;
                }
                if (expressionParts[i].Contains("!"))
                {
                    double first = double.Parse(expressionParts[i].Substring(0, expressionParts[i].Length - 1).Replace("neg", "-"));
                    IModOrFactorial proxy = (IModOrFactorial)XmlRpcProxyGen.Create(typeof(IModOrFactorial));
                    int first2 = Convert.ToInt32(first);
                    Result ret = proxy.Factorial(first2);
                    partialResult = ret.result;
                }

                expressionParts[i] = partialResult.ToString();
            }

            for (int i = 0; i < expressionParts.Count(); i++)
            {
                expression = expression.Replace("arg"+(char)('a'+i), expressionParts[i]);
            }

            expression = BasicOperator(expression, '*');

            expression = BasicOperator(expression, '/');

            expression = BasicOperator(expression, '+');

            expression = BasicOperator(expression, '-');

            return double.Parse(expression);

        }

        public static string BasicOperator(string expression, char c)
        {
            while (expression.Contains(c))
            {
                if (expression[0] == '-')
                {
                    expression = expression.Substring(1, expression.Length - 1);
                    expression = expression.Insert(0, "neg");
                }
                expression = expression.Replace("*-", "*neg");
                expression = expression.Replace("/-", "/neg");
                expression = expression.Replace("+-", "+neg");
                expression = expression.Replace("--", "-neg");


                try
                {
                    double f = double.Parse(expression.Replace("neg", "-"));
                    return f.ToString();
                }
                catch(Exception e)
                {

                }
                int startIndex = -1;
                int endIndex = -1;
                for (int i = expression.IndexOf(c) - 1; i >= 0; i--)
                {
                    if (expression[i] == '*' || expression[i] == '+' || expression[i] == '/' || expression[i] == '-')
                    {
                        startIndex = i+1;
                        break;
                    }
                    else { startIndex = 0; }
                }
                for (int i = expression.IndexOf(c) + 1; i < expression.Length; i++)
                {
                    if (expression[i] == '*' || expression[i] == '+' || expression[i] == '/' || expression[i] == '-')
                    {
                        endIndex = i;
                        break;
                    }
                    else { endIndex = expression.Length; }
                }
                string firstString = expression.Substring(startIndex, expression.IndexOf(c) - startIndex);
                firstString = firstString.Replace("neg", "-");
                double first = double.Parse(firstString);
                int start = expression.IndexOf(c) + 1;
                int end = endIndex - expression.IndexOf(c) - 1;
                string endString = expression.Substring(start, end);
                endString = endString.Replace("neg", "-");
                double second = double.Parse(endString);
                switch (c)
                {
                    case '*':
                        expression = expression.Replace(expression.Substring(startIndex, endIndex - startIndex), (first * second).ToString());
                        break;
                    case '+':
                        expression = expression.Replace(expression.Substring(startIndex, endIndex - startIndex), (first + second).ToString());
                        break;
                    case '-':
                        expression = expression.Replace(expression.Substring(startIndex, endIndex - startIndex), (first - second).ToString());
                        break;
                    case '/':
                        expression = expression.Replace(expression.Substring(startIndex, endIndex - startIndex), (first / second).ToString());
                        break;
                    default:
                        break;

                }
            }
            return expression;
        }

        public static string ReplaceFirst(string expression, string searchString, string replacement)
        {
            int pos = expression.IndexOf(searchString);
            if (pos < 0)
            {
                return expression;
            }
            return expression.Substring(0, pos) + replacement + expression.Substring(pos + searchString.Length);
        }
    }
}
