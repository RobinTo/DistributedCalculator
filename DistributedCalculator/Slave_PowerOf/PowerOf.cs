using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Slave_PowerOf
{
    public struct Result
    {
        public double result;
    }

    class PowerOfClass : MarshalByRefObject
    {
        [XmlRpcMethod("PowerOf")]
        public Result PowerOf(double x, double y)
        {
            Result ret;
            ret.result = Math.Pow(x,y);
            Console.WriteLine(x.ToString() + "^" + y.ToString() + "=" + ret.result);
            return ret;
        }
    }
}
