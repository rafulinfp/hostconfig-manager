using System;
using HostconfigManager.Core.Services;

namespace HostconfigManager.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                var targetEnv = args[0];
                System.Console.WriteLine($"Updating hostconfig with '{targetEnv}' environment info");
                var environment = new EnvironmentConfig();
                var result = environment.SetEnvironment(targetEnv);
                System.Console.WriteLine($"Result {result}");
            }
            else
            {
                System.Console.WriteLine("No environment has been specified!");
            }
        }
    }
}
