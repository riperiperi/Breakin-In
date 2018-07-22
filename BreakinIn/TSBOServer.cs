using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn
{
    public class TSBOServer
    {
        AbstractEAServer Redirector;
        AbstractEAServer Matchmaker;

        public bool Run(string addr)
        {
            Console.WriteLine("Breakin' In... ("+addr+")");

            Console.WriteLine("=== Listeners ===");

            try
            {
                Redirector = new RedirectorServer(11100, addr, 10901);
                Console.WriteLine("Redirector: OK!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Redirector: Failed to start! Exception:");
                Console.WriteLine(e.ToString());
                return false;
            }

            try
            {
                Redirector = new MatchmakerServer(10901);
                Console.WriteLine("Matchmaker: OK!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Matchmaker: Failed to start! Exception:");
                Console.WriteLine(e.ToString());
                Redirector.Dispose();
                return false;
            }

            //main loop? no purpose right now
            Console.WriteLine("=================");

            return true;
        }
    }
}
