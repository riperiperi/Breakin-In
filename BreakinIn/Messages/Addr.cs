using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class Addr : AbstractMessage
    {
        public override string _Name { get => "addr"; }

        public string ADDR { get; set; }
        public string PORT { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            client.Port = PORT;
        }
    }
}
