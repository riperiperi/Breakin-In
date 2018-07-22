using BreakinIn.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Model
{
    public class User
    {
        public int ID;
        public Game CurrentGame;
        public Room CurrentRoom;
        public EAClient Connection;
        public string Username;
        public string[] Personas = new string[4];
        public string Auxiliary = "";
        public string IP = "127.0.0.1";

        public int SelectedPersona = -1;
        public string PersonaName { get => (SelectedPersona == -1) ? null : (Personas[SelectedPersona]); }

        public void SelectPersona(string name)
        {
            SelectedPersona = Array.IndexOf(Personas, name);
        }

        public PlusUser GetInfo()
        {
            return new PlusUser()
            {
                I = ID.ToString(),
                N = PersonaName ?? "",
                M = Username,
                A = IP,
                X = Auxiliary,
                G = (CurrentGame?.ID ?? 0).ToString(),
                P = Connection.Ping.ToString()
            };
        }
    }
}
