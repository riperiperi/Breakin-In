using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class PersIn : AbstractMessage
    {
        public override string _Name { get => "pers"; }

        public string PERS { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var user = client.User;
            if (user == null) return;
            user.SelectPersona(PERS);
            if (user.SelectedPersona == -1) return; //failed?
            client.SendMessage(new PersOut() {
                NAME = user.Username,
                PERS = user.PersonaName
            });
        }
    }
}
