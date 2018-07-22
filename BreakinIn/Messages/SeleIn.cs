using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class SeleIn : AbstractMessage
    {
        public override string _Name { get => "sele"; }

        public string ROOMS { get; set; } = "1";

        public override void Process(AbstractEAServer context, EAClient client)
        {
            //TODO: provide some actual statistics
            client.SendMessage(new SeleOut());
        }
    }
}
