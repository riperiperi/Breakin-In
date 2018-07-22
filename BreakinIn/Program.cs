using System;

namespace BreakinIn
{
    public class Program
    {
        static void Main(string[] args)
        {
            var server = new TSBOServer();
            var result = server.Run(JSONConfig.Inst.external_ip);
            if (!result)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
