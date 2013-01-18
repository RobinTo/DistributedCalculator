using System;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Slave_Trigonometry
{
    public struct Result
    {
        public double result;
    }

    class Trigonometry : MarshalByRefObject
    {
        [XmlRpcMethod("Sinus")]
        public Result Sinus(double x)
        {
            Result ret;
            ret.result = Math.Sin(x);
            Console.WriteLine("Sin("+x.ToString()+")="+ret.result);
            return ret;
        }

        [XmlRpcMethod("Cosinus")]
        public Result Cosinus(double x)
        {
            Result ret;
            ret.result = Math.Cos(x);
            Console.WriteLine("Cos(" + x.ToString() + ")=" + ret.result);
            return ret;
        }
    }
}
