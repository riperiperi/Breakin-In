using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class UserIn : AbstractMessage
    {
        public override string _Name { get => "user"; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            //TODO: provide actual user info
            var user = client.User;
            if (user == null) return;

            var result = new UserOut()
            {
                MESG = user.Username,
                ADDR = user.IP,
            };

            client.SendMessage(result);
        }
    }
}
