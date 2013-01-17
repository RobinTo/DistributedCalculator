using System;
using System.Collections.Generic;
using CookComputing.XmlRpc;


namespace Master
{
    public struct Result
    {
        public double result;
    }

    [XmlRpcUrl("http://127.0.0.1:1337/modOrFactorial.rem")]
    public interface IModOrFactorial : IXmlRpcProxy
    {
        [XmlRpcMethod]
        Result Modulus(double x, double y);

        [XmlRpcMethod]
        Result Factorial(int x);
    }

    [XmlRpcUrl("http://127.0.0.1:1338/powerOf.rem")]
    public interface IPowerOf : IXmlRpcProxy
    {
        [XmlRpcMethod]
        Result PowerOf(double x, double y);
    }

    [XmlRpcUrl("http://127.0.0.1:1339/trigonometry.rem")]
    public interface ITrigonometry : IXmlRpcProxy
    {
        [XmlRpcMethod]
        Result Cosinus(double x);

        [XmlRpcMethod]
        Result Sinus(double x);
    }
}
