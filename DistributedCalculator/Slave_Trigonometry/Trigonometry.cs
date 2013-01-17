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
            return ret;
        }

        [XmlRpcMethod("Cosinus")]
        public Result Cosinus(double x)
        {
            Result ret;
            ret.result = Math.Cos(x);
            return ret;
        }
    }
}
