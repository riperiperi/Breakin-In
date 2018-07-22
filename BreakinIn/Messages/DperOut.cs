using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class DperOut : AbstractMessage
    {
        public override string _Name { get => "dper"; }

        public string NAME { get; set; }
        public string PERS { get; set; }
    }
}
