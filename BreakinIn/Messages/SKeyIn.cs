using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class SKeyIn : AbstractMessage
    {
        public override string _Name { get => "skey"; }
        public string SKEY { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            //TODO: get actual session key
            client.SendMessage(new SKeyOut());
        }
    }
}
