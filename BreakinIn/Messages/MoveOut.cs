using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class MoveOut : AbstractMessage
    {
        public override string _Name { get => "move"; }

        public string IDENT { get; set; }
        public string NAME { get; set; }
        public string COUNT { get; set; }
        public string FLAGS { get; set; } = "C";
    }
}
