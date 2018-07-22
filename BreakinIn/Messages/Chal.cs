using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BreakinIn.Model;

namespace BreakinIn.Messages
{
    public class Chal : AbstractMessage
    {
        public override string _Name { get => "chal"; }

        public string HOST { get; set; }
        public string KIND { get; set; }
        public string PERS { get; set; }

        public string _From;
        public User _FromUser;

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var user = client.User;
            if (user == null) return;

            var room = user.CurrentRoom;
            if (room == null) return;

            _From = user.PersonaName;
            _FromUser = user;
            
            lock (room.ChallengeMap)
            {
                if (PERS == "*")
                {
                    //we don't want to play with anyone anymore
                    var old = room.ChallengeMap.Values.FirstOrDefault(x => x._From == _From);
                    if (old != null)
                    {
                        room.ChallengeMap.Remove(old.PERS);
                        return;
                    }
                }
                //firstly, is someone wanting to play with us yet
                else if (room.ChallengeMap.ContainsKey(user.PersonaName))
                {
                    //and we want to play with them?
                    var other = room.ChallengeMap[user.PersonaName];
                    if (PERS == other._From)
                    {
                        //start the session.
                        var chals = new Chal[] { this, other };
                        Console.WriteLine("Starting a game session between " + _From + " and " + this.PERS);
                        var host = chals.FirstOrDefault(x => x.HOST == "1");
                        var users = chals.Select(x => x._FromUser);
                        if (host == null) return; //??

                        var sess = new PlusSes()
                        {
                            IDENT = "1",
                            HOST = host._From,
                            ROOM = room.Name,
                            KIND = host.KIND,

                            OPID = users.Select(x => x.ID.ToString()).ToArray(),
                            OPPO = users.Select(x => x.PersonaName).ToArray(),
                            ADDR = users.Select(x => x.IP).ToArray(),

                            SEED = (new Random()).Next().ToString(),
                            SELF = user.PersonaName
                        };

                        foreach (var userx in users)
                        {
                            sess.SELF = userx.PersonaName;
                            userx.Connection?.SendMessage(sess);
                        }
                        return;
                    }
                }

                //otherwise let's add this 
                room.ChallengeMap[PERS] = this;
            }
        }
    }
}
