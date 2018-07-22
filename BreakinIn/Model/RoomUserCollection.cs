using BreakinIn.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Model
{
    public class RoomUserCollection : UserCollection
    {
        public Room Room;

        public RoomUserCollection(Room parent)
        {
            Room = parent;
        }
        
        public override void AddUser(User user)
        {
            base.AddUser(user);

            //send move to this user
            MoveOut move;
            lock (Users)
            {
                move = new MoveOut()
                {
                    IDENT = Room.ID.ToString(),
                    NAME = Room.Name,
                    COUNT = Users.Count.ToString()
                };
            }
            user.Connection?.SendMessage(move);

            //send who to this user to tell them who they are

            var info = user.GetInfo();
            var who = new PlusWho()
            {
                I = info.I,
                N = info.N,
                M = info.M,
                A = info.A,
                X = info.X,
                R = Room.Name,
                RI = Room.ID.ToString()
            };

            user.Connection?.SendMessage(who);
            RefreshUser(user);
            ListToUser(user);
            Room.BroadcastPopulation();
        }

        public void RefreshUser(User target)
        {
            var info = target.GetInfo();
            Broadcast(info);
        }

        public void ListToUser(User target)
        {
            List<PlusUser> infos = new List<PlusUser>();
            lock (Users)
            {
                foreach (var user in Users)
                {
                    infos.Add(user.GetInfo());
                }
            }
            foreach (var info in infos) target.Connection?.SendMessage(info);
        }

        public override void RemoveUser(User user)
        {
            base.RemoveUser(user);
            Broadcast(new PlusUser()
            {
                I = user.ID.ToString(),
                T = "1",
                F = null,
                P = null,
                S = null
            });

            Broadcast(new PlusMesg()
            {
                F = "C",
                T = "\"has left the room\"",
                N = user.PersonaName
            });

            Room.BroadcastPopulation();
        }
    }
}
