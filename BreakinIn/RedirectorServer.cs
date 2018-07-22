using BreakinIn.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn
{
    public class RedirectorServer : AbstractEAServer
    {
        public override Dictionary<string, Type> NameToClass { get; } =
            new Dictionary<string, Type>()
            {
                { "@dir", typeof(DirIn) }
            };

        public string RedirIP;
        public string RedirPort;

        public RedirectorServer(ushort port, string targetIP, ushort targetPort) : base(port)
        {
            RedirIP = targetIP;
            RedirPort = targetPort.ToString();
        }
    }
}
