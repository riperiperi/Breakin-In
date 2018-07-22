using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class MoveIn : AbstractMessage
    {
        public override string _Name { get => "move"; }
        
        public string NAME { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var user = client.User;
            if (user == null) return;

            if (user.CurrentRoom != null)
            {
                user.CurrentRoom.Users.RemoveUser(user);
                user.CurrentRoom = null;
            }

            var room = mc.Rooms.GetRoomByName(NAME);
            if (room != null)
            {
                room.Users.AddUser(user);
                user.CurrentRoom = room;
            }
            else
            {
                client.SendMessage(new MoveOut()
                {
                    NAME = ""
                });
            }
        }
    }
}
