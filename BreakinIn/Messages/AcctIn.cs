using BreakinIn.DataStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class AcctIn : AbstractMessage
    {
        public override string _Name { get => "acct"; }

        public string NAME { get; set; }
        public string PASS { get; set; } = "";

        public string MAIL { get; set; }
        public string BORN { get; set; }
        public string GEND { get; set; }
        public string SPAM { get; set; }
        public string ALTS { get; set; }
        public string FROM { get; set; }
        public string LANG { get; set; }
        public string PROD { get; set; }
        public string VERS { get; set; }
        public string SLUS { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var info = new DbAccount()
            {
                Username = NAME,
                Password = PASS,
            };

            var created = mc.Database.CreateNew(info);
            if (created) {
                client.SendMessage(new AcctOut()
                {
                    NAME = NAME,
                    PERSONAS = "",
                    AGE = "24"
                });
            } else {
                client.SendMessage(new AcctDupl());
            }
        }
    }

    public class AcctDupl : AbstractMessage
    {
        public override string _Name { get => "acctdupl"; }
    }
}
