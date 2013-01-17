using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Slave_Trigonometry
{
    class Program
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("Trigonometry.exe.config", false);
            RemotingConfiguration.RegisterWellKnownServiceType(
              typeof(Trigonometry),
              "Trigonometry.rem",
              WellKnownObjectMode.Singleton);
            Console.WriteLine("Press to shutdown");
            Console.ReadLine(); 
        }
    }
}
