using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class Mesg : AbstractMessage
    {
        public override string _Name { get => "mesg"; }

        public string PRIV { get; set; }
        public string TEXT { get; set; } = "";
        public string ATTR { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null || !client.HasAuth()) return;
            var user = client.User;
            var mesg = new PlusMesg()
            {
                N = user.PersonaName,
                T = TEXT,
            };

            //where is this message going
            var room = user.CurrentRoom;
            if (PRIV != null)
            {
                if (ATTR != null && ATTR.Length > 1 && ATTR[0] == 'N')
                {
                    mesg.F = "EP" + ATTR.Substring(1);
                }
                mc.SendToPersona(PRIV, mesg);
            } else if (room != null)
            {
                room.Users.Broadcast(mesg);
            }
        }
    }
}
