using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BreakinIn.Messages;

namespace BreakinIn.Model
{
    public class RoomCollection
    {
        public MatchmakerServer Server;
        private int RoomID = 1;
        private List<Room> Rooms = new List<Room>();

        public virtual void AddRoom(Room room)
        {
            lock (Rooms)
            {
                room.Server = Server;
                room.ID = RoomID++;
                Rooms.Add(room);
            }
        }

        public virtual void RemoveRoom(Room room)
        {
            lock (Rooms)
            {
                Rooms.Remove(room);
            }
        }

        public Room GetRoomByName(string name)
        {
            lock (Rooms)
            {
                return Rooms.FirstOrDefault(x => x.Name == name);
            }
        }

        public int Count()
        {
            lock (Rooms)
            {
                return Rooms.Count;
            }
        }

        public void SendRooms(User user)
        {
            var infos = new List<PlusRom>();
            lock (Rooms)
            {
                foreach (var room in Rooms)
                {
                    infos.Add(room.GetInfo());
                }
            }
            foreach (var info in infos) user.Connection?.SendMessage(info);
        }
    }
}
