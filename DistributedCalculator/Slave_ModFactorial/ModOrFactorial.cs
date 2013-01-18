using System;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Slave_ModFactorial
{
    public struct Result
    {
        public double result;
    }

    public class ModOrFactorial : MarshalByRefObject
    {
        [XmlRpcMethod("Modulus")]
        public Result Modulus(double x, double y)
        {
            Result ret;
            ret.result = x % y;
            Console.WriteLine(x.ToString() + "mod" + y.ToString() + "=" + ret.result);
            return ret;
        }

        [XmlRpcMethod("Factorial")]
        public Result Factorial(int x)
        {
            Result ret;
            ret.result = 1;

            if (x > 0)
            {
                for (int i = x; i > 0; i--)
                {
                    ret.result *= i;
                }
            }
            else if (x < 0)
            {
                for (int i = x; i < 0; i++)
                {
                    ret.result *= i;
                }
                ret.result = Math.Abs(ret.result) * -1; // 4! = -24, 3! = -6 ....
            }
            else
                ret.result = 0;

            Console.WriteLine(x.ToString() + "!" + "=" + ret.result);

            return ret;
        }
    }

    
}
