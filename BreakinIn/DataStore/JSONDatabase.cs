using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BreakinIn.DataStore
{
    public class JSONDatabase : IDatabase
    {
        public int AutoInc = 1;
        public List<DbAccount> Accounts = new List<DbAccount>();
        public HashSet<string> Personas = new HashSet<string>();

        public JSONDatabase()
        {
            //load from disk.
            Console.WriteLine("Loading Database...");

            try
            {
                using (var file = File.Open("db.json", FileMode.Open, FileAccess.Read, FileShare.None))
                using (var io = new StreamReader(file))
                {
                    var json = io.ReadToEnd();
                    Accounts = JsonConvert.DeserializeObject<List<DbAccount>>(json);
                }
                Personas.Clear();
                foreach (var user in Accounts)
                {
                    foreach (var persona in user.Personas)
                    {
                        Personas.Add(persona);
                    }
                }
                if (Accounts.Count > 0)
                    AutoInc = Accounts.Max(x => x.ID) + 1;
            } catch
            {
                Console.WriteLine("Error loading database! Starting with a blank slate.");
                Save();
            }
        }

        public bool CreateNew(DbAccount info)
        {
            if (info.Username == null) return false;
            lock (Accounts)
            {
                if (GetByName(info.Username) != null) return false; //already exists
                info.ID = AutoInc++;
                Accounts.Add(info);
                Save();
            }
            return true;
        }

        public DbAccount GetByName(string username)
        {
            lock (Accounts)
            {
                return Accounts.FirstOrDefault(x => x.Username == username);
            }
        }

        public int AddPersona(int id, string persona)
        {
            var regex = new Regex(@"[a-zA-Z0-9\s]");
            if (!regex.IsMatch(persona)) return -1;
            var index = 0;
            lock (Accounts)
            {
                var acct = Accounts.FirstOrDefault(x => x.ID == id);
                if (acct == null || acct.Personas.Count == 4) return -1;
                if (Personas.Contains(persona)) return -2;
                Personas.Add(persona);
                acct.Personas.Add(persona);
                index = acct.Personas.Count;
                Save();
            }
            return index;
        }

        public int DeletePersona(int id, string persona)
        {
            var index = 0;
            lock (Accounts)
            {
                var acct = Accounts.FirstOrDefault(x => x.ID == id);
                if (acct == null) return -1;
                index = acct.Personas.IndexOf(persona);
                if (index != -1)
                {
                    Personas.Remove(persona);
                    acct.Personas.Remove(persona);
                    Save();
                }
            }
            return index;
        }

        private void Save()
        {
            string data;
            lock (Accounts)
            {
                data = JsonConvert.SerializeObject(Accounts);
            }
            using (var file = File.Open("db.json", FileMode.Create, FileAccess.Write, FileShare.None))
                using (var io = new StreamWriter(file))
            {
                io.Write(data);
            }
        }
    }
}
