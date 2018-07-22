using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Model
{
    public class Game
    {
        public int Max;
        public int Min;
        public int Count;
        public int ID;
        public int SysFlags;

        public string Params;
        public string Name;

        public UserCollection Users = new UserCollection();
    }
}
