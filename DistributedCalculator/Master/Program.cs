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
            string expression = "((8mod6)*(5/4)+4+6)(9^4)(cos(2)+sin(2))";
            Console.WriteLine("Calculating: " + expression);
            expression = expression.Replace(".", ",");
            expression = expression.Replace(" ", String.Empty);
            while(expression.Contains(")("))
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
                    expression = expression.Insert(left, (char)('a'+(paranthesis.Count - 1))+"arg");
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

            Console.ReadLine();
        }

        public static float CalculateExpression(string expression, List<string> paranthesis)
        {
            float partialResult = 0;

            char[] operators = new char[] { '-', '+', '*', '/' };

            while (expression.Contains("arg"))
            {
                char counter = (expression.Substring(expression.IndexOf("arg")-1, 1))[0];
                expression = expression.Replace(counter + "arg", paranthesis[(int)(counter-97)]);
            }
            
            
            

            string[] expressionParts = expression.Split(operators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expressionParts.Count(); i++)
            {
                expression = expression.Replace(expressionParts[i], "arg" + (char)('a'+i));
            }

            for (int i = 0; i < expressionParts.Count(); i++)
            {
                partialResult = 0;
                try
                {
                    partialResult = float.Parse(expressionParts[i]);
                }
                catch (Exception e)
                { }
                if (expressionParts[i].Contains("mod"))
                {
                    float first = float.Parse(expressionParts[i].Substring(0, expressionParts[i].IndexOf("mod")));
                    float second = float.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("mod") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("mod") + 3)));

                    IModOrFactorial proxy = (IModOrFactorial)XmlRpcProxyGen.Create(typeof(IModOrFactorial));
                    Result ret = proxy.Modulus((double)first, (double)second);
                    partialResult = (float)ret.result;
                }
                if (expressionParts[i].Contains("cos"))
                {
                    float cosNumber = float.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("cos") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("cos") + 3)));
                    ITrigonometry proxy = (ITrigonometry)XmlRpcProxyGen.Create(typeof(ITrigonometry));
                    Result ret = proxy.Cosinus((double)cosNumber);
                    partialResult = (float)ret.result;
                }
                if (expressionParts[i].Contains("sin"))
                {
                    float sinNumber = float.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("sin") + 3, expressionParts[i].Length - (expressionParts[i].IndexOf("sin") + 3)));
                    ITrigonometry proxy = (ITrigonometry)XmlRpcProxyGen.Create(typeof(ITrigonometry));
                    Result ret = proxy.Sinus((double)sinNumber);
                    partialResult = (float)ret.result;
                }
                if (expressionParts[i].Contains("^"))
                {
                    float first = float.Parse(expressionParts[i].Substring(0, expressionParts[i].IndexOf("^")));
                    float second = float.Parse(expressionParts[i].Substring(expressionParts[i].IndexOf("^") + 1, expressionParts[i].Length - (expressionParts[i].IndexOf("^") + 1)));
                    IPowerOf proxy = (IPowerOf)XmlRpcProxyGen.Create(typeof(IPowerOf));
                    Result ret = proxy.PowerOf((double)first, (double)second);
                    partialResult = (float)ret.result;
                }
                if (expressionParts[i].Contains("!"))
                {
                    // Insert factorial parsing thingies here.
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

            return float.Parse(expression);

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
                    float f = float.Parse(expression.Replace("neg", "-"));
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
                        startIndex = i;
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
                float first = float.Parse(firstString);
                int start = expression.IndexOf(c) + 1;
                int end = endIndex - expression.IndexOf(c) - 1;
                string endString = expression.Substring(start, end);
                endString = endString.Replace("neg", "-");
                float second = float.Parse(endString);
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
    }
}
