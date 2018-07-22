using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    class PlusWho : AbstractMessage
    {
        public override string _Name { get => "+who"; }

        public string I { get; set; }
        public string N { get; set; }
        public string M { get; set; }
        public string F { get; set; } = "";
        public string A { get; set; }
        public string S { get; set; } = "";
        public string X { get; set; }
        public string R { get; set; } //room
        public string RI { get; set; } //room id
        public string RF { get; set; } = "C";
        public string RT { get; set; } = "1";
    }
}
