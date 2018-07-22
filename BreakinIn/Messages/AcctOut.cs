using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class AcctOut : AbstractMessage
    {
        public override string _Name { get => "acct"; }

        public string NAME { get; set; }
        public string PERSONAS { get; set; }
        public string AGE { get; set; }
    }
}
