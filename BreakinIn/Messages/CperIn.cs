using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class CperIn : AbstractMessage
    {
        public override string _Name { get => "cper"; }
        
        public string PERS { get; set; }
        public string ALTS { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null || !client.HasAuth()) return;

            var index = mc.Database.AddPersona(client.User.ID, PERS);
            if (index < 0)
            {
                if (index == -2) client.SendMessage(new CperDupl());
                else client.SendMessage(new CperImst());
                return;
            }
            var user = client.User;
            user.Personas[index] = PERS;

            client.SendMessage(new CperOut()
            {
                NAME = user.Username,
                PERS = PERS
            });
        }

        public class CperDupl : AbstractMessage
        {
            public override string _Name { get => "cperdupl"; }
        }

        public class CperImst : AbstractMessage
        {
            public override string _Name { get => "cperimst"; }
        }
    }
}
