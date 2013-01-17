using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Slave_PowerOf
{
    class Program
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("PowerOf.exe.config", false);
            RemotingConfiguration.RegisterWellKnownServiceType(
              typeof(PowerOfClass),
              "PowerOf.rem",
              WellKnownObjectMode.Singleton);
            Console.WriteLine("Press to shutdown");
            Console.ReadLine(); 
        }
    }
}
