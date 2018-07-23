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
                room.RemoveChallenges(user); //remove any challenges we made before
                if (PERS == "*")
                {
                    //we don't want to play with anyone anymore
                    return;
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
                        var host = chals.FirstOrDefault(x => x.HOST == "1");
                        var users = chals.Select(x => x._FromUser);
                        if (host == null) return; //??

                        if (room.AllInGame)
                        {
                            users = room.Users.GetAll();
                            Console.WriteLine("Starting an all play game session: " + string.Join(',', users.Select(x => x.PersonaName)));
                        }
                        else
                        {
                            Console.WriteLine("Starting a private game session between " + _From + " and " + this.PERS);
                        }

                        var sess = new PlusSes()
                        {
                            IDENT = "1",
                            HOST = host._From,
                            ROOM = room.Name,
                            KIND = host.KIND,
                            COUNT = users.Count().ToString(),

                            OPID = users.Select(x => x.ID.ToString()).ToArray(),
                            OPPO = users.Select(x => x.PersonaName).ToArray(),
                            ADDR = users.Select(x => x.IP).ToArray(),

                            SEED = (new Random()).Next().ToString(),
                            SELF = user.PersonaName
                        };

                        /*
                         * Experimental stuff to try get bustin out to connect multiple users
                         * didn't work unfortunately.
                        if (room.AllInGame)
                        {
                            //send a packet containing everyone to the host
                            sess.SELF = host._From;
                            var hostuser = host._FromUser;
                            hostuser.Connection?.SendMessage(sess);

                            //send a packet containing only the host and self to everyone else
                            foreach (var userx in users)
                            {
                                if (userx == hostuser) continue;
                                sess.SELF = userx.PersonaName;

                                var nusers = new List<User>() { hostuser, userx };

                                sess.COUNT = "2";
                                sess.OPID = nusers.Select(x => x.ID.ToString()).ToArray();
                                sess.OPPO = nusers.Select(x => x.PersonaName).ToArray();
                                sess.ADDR = nusers.Select(x => x.IP).ToArray();

                                userx.Connection?.SendMessage(sess);
                            }
                        }
                        else
                        {*/
                            foreach (var userx in users)
                            {
                                sess.SELF = userx.PersonaName;
                                userx.Connection?.SendMessage(sess);
                            }
                        //}

                        return;
                    }
                }

                //otherwise let's add this 
                room.ChallengeMap[PERS] = this;
            }
        }
    }
}
