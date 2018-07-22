using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class PlusPop : AbstractMessage
    {
        public override string _Name { get => "+pop"; }

        public string Z { get; set; }
    }
}
