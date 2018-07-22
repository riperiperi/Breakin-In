using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.DataStore
{
    public class DbAccount
    {
        public int ID;
        public string Username;
        public string Password; //todo: hash
        public List<string> Personas = new List<string>();
    }
}
