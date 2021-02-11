using BreakinIn.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    class OnlnIn : AbstractMessage
    {
        public override string _Name { get => "onln"; }

        public string PERS { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var user = client.User;
            if (user == null) return;

            var info = user.GetInfo();
            client.SendMessage(info);

            var OtherPlayer = mc.Users.GetUserByPersonaName(PERS);


            if (OtherPlayer == null)
            {
                client.SendMessage(new OnlnOut());
                return;
            }

            if (OtherPlayer.PersonaName == user.PersonaName)
            {
                //There doesn't seem to be any error types or messages for searching for yourself.
                client.SendMessage(new OnlnOut()
                {
                    N = user.PersonaName,
                });
                return;
            }
            if (OtherPlayer.CurrentRoom != null)
            {
                client.SendMessage(new OnlnOut()
                {
                    //Other player is online and are in a room.
                    N = OtherPlayer.PersonaName,
                    RM = OtherPlayer.CurrentRoom.Name,
                });
                return;
            }
            else if (OtherPlayer.CurrentRoom == null)
            {
                client.SendMessage(new OnlnOut()
                {
                    //Other player isn't in a room, but they are online in the main lobby.
                    N = OtherPlayer.PersonaName,
                });
                return;
            }
        }
    }
}
