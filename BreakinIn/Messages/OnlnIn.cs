using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    class OnlnIn : AbstractMessage
    {
        public override string _Name { get => "onln"; }

        public string PERS { get; set; }
        public string ROOM { get; set; } = "Room-A";

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var user = client.User;
            if (user == null) return;

            var Room = user.CurrentRoom;

            var info = user.GetInfo();
            
            client.SendMessage(info);
            client.SendMessage(this);
            //client.SendMessage(new OnlnImst());
        }
    }

    class OnlnImst : AbstractMessage
    {
        public override string _Name { get => "onlnimst"; }
    }
}
