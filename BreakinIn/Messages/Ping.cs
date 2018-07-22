using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class Ping : AbstractMessage
    {
        public override string _Name { get => "~png"; }

        public string TIME { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            client.Ping = (int)(new TimeSpan(DateTime.Now.Ticks - client.PingSendTick).TotalMilliseconds);
        }
    }
}
