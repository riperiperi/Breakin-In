using BreakinIn.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Model
{
    public class Room
    {
        public MatchmakerServer Server;
        public int ID;
        public string Name;
        public RoomUserCollection Users;
        public List<Game> Games = new List<Game>();
        public bool IsGlobal; //if a room is global, it is always open
        public int Max = 4;

        public Dictionary<string, Chal> ChallengeMap = new Dictionary<string, Chal>();

        public Room()
        {
            Users = new RoomUserCollection(this);
        }

        public PlusRom GetInfo()
        {
            return new PlusRom()
            {
                I = ID.ToString(),
                N = Name,
                T = Users.Count().ToString(),
                L = Max.ToString()
            };
        }

        public void BroadcastPopulation()
        {
            Server.Users.Broadcast(new PlusPop() { Z = Users.Count() + "/" + Max });
        }
    }
}
