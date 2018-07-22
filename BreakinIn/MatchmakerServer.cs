using BreakinIn.DataStore;
using BreakinIn.Messages;
using BreakinIn.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BreakinIn
{
    public class MatchmakerServer : AbstractEAServer
    {
        public override Dictionary<string, Type> NameToClass { get; } =
            new Dictionary<string, Type>()
            {
                { "move", typeof(MoveIn) }, //move into a room. remove last room and broadcast "+rom" to others. broadcast "+pop" with Z=ID/Count for population update
                { "mesg", typeof(Mesg) }, //PRIV is non-null for private. else broadcast to room. PRIV->(find client), TEXT->T, ATTR->EP, (name)->N
                { "room", null }, //create room. NAME=name, return (room, +who, +msg, +rom to all, +usr)
                { "auxi", typeof(Auxi) }, //auxiliary information. returned as X attribute in +usr and +who
                { "auth", typeof(AuthIn) },
                { "acct", typeof(AcctIn) },
                { "~png", typeof(Ping) },
                { "cper", typeof(CperIn) }, //create persona. (NAME) in, (PERS, NAME) out. where name is username.
                { "dper", typeof(DperIn) }, //delete persona
                { "pers", typeof(PersIn) }, //select persona
                { "skey", typeof(SKeyIn) }, //session key?
                { "sele", typeof(SeleIn) }, //gets info for the current server
                { "user", typeof(UserIn) }, //get my user info
                { "onln", typeof(OnlnIn) }, //search for a user's info
                { "addr", typeof(Addr) }, //the client tells us their IP and port (ephemeral). The IP is usually wrong.

                { "chal", typeof(Chal) }, //enter challenge mode

                //need for speed
                { "glea", null }, //leave game (string NAME)
                { "gget", null }, //get game by name
                { "gjoi", null }, //join game by name
                { "gdel", null }, //delete game by name
                { "gsta", null }, //game start. return gstanepl if not enough people, empty gsta if enough.
                { "gcre", null }, //game create. (name, roomname, maxPlayers, minPlayers, sysFlags, params). return a lot of info
                { "news", null }, //news for server. return newsnew0 with news info (plaintext mode, NOT keyvalue)

                { "rank", null }, //unknown. { RANK = "Unranked", TIME = 866 }
            };

        public UserCollection Users = new UserCollection();
        public RoomCollection Rooms = new RoomCollection();

        public IDatabase Database;
        private Thread PingThread;

        public MatchmakerServer(ushort port) : base(port)
        {
            Database = new JSONDatabase();
            Rooms.Server = this;

            PingThread = new Thread(PingLoop);
            PingThread.Start();

            Rooms.AddRoom(new Room() { Name = "Room-E", IsGlobal = true });
            Rooms.AddRoom(new Room() { Name = "Room-D", IsGlobal = true });
            Rooms.AddRoom(new Room() { Name = "Room-C", IsGlobal = true });
            Rooms.AddRoom(new Room() { Name = "Room-B", IsGlobal = true });
            Rooms.AddRoom(new Room() { Name = "Room-A", IsGlobal = true });
        }

        public void PingLoop()
        {
            while (true)
            {
                Broadcast(new Ping());
                Thread.Sleep(30000);
            }
        }

        public override void RemoveClient(EAClient client)
        {
            base.RemoveClient(client);

            //clean up this user's state.
            //are they logged in?
            var user = client.User;
            if (user != null)
            {
                Users.RemoveUser(user);

                var game = user.CurrentGame;
                var room = user.CurrentRoom;
                if (game != null)
                {
                    game.Users.RemoveUser(user);
                    user.CurrentGame = null;
                }

                if (room != null)
                {
                    room.Users.RemoveUser(user);
                    user.CurrentRoom = null;
                }
            }
        }

        public void SendToPersona(string name, AbstractMessage msg)
        {
            var user = Users.GetUserByPersonaName(name);
            if (user != null)
            {
                user.Connection?.SendMessage(msg);
            }
        }

        public void TryLogin(DbAccount user, EAClient client)
        {
            //is someone else already logged in as this user?
            var oldUser = Users.GetUserByName(user.Username);
            if (oldUser != null)
            {
                oldUser.Connection.Close(); //should remove the old user.
                Thread.Sleep(500);
            }

            var personas = new string[4];
            for (int i=0; i<user.Personas.Count; i++)
            {
                personas[i] = user.Personas[i];
            }

            //make a user object from DB user
            var user2 = new User()
            {
                Connection = client,
                ID = user.ID,
                Personas = personas,
                Username = user.Username,
                IP = client.IP
            };

            Users.AddUser(user2);
            client.User = user2;

            client.SendMessage(new AuthOut()
            {
                TOS = user.ID.ToString(),
                NAME = user.Username,
                PERSONAS = string.Join(',', user.Personas)
            });

            Rooms.SendRooms(user2);
        }
    }
}
