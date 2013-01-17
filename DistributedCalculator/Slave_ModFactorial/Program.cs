using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using CookComputing.XmlRpc;

namespace Slave_ModFactorial
{
    class Program
    {
        static void Main(string[] args)
        {

            // for CookComputing.XmlRpcV2
            RemotingConfiguration.Configure("ModOrFactorial.exe.config", false);
            // for CookComputing.XmlRpc
            //RemotingConfiguration.Configure("SumAndDiff.exe.config"); 
            RemotingConfiguration.RegisterWellKnownServiceType(
              typeof(ModOrFactorial),
              "ModOrFactorial.rem",
              WellKnownObjectMode.Singleton);
            Console.WriteLine("Press to shutdown");
            Console.ReadLine(); 
        }
    }
}
