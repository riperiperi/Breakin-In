using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class AuthIn : AbstractMessage
    {
        public override string _Name { get => "auth"; }

        public string NAME { get; set; }
        public string PASS { get; set; }
        public string TOS { get; set; }
        public string MID { get; set; }
        public string FROM { get; set; } = "US";
        public string LANG { get; set; } = "en";

        public string PROD { get; set; }
        public string VERS { get; set; }
        public string SLUS { get; set; }
        public string REGN { get; set; }
        public string CLST { get; set; }
        public string NETV { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;


            var user = mc.Database.GetByName(NAME);
            if (user == null)
            {
                client.SendMessage(new AuthImst());
                return;
            }

            mc.TryLogin(user, client);
        }
    }
}
