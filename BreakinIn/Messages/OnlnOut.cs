using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class OnlnOut : AbstractMessage
    {
        public override string _Name { get => "onln"; }

        public string N { get; set; }
        public string RM { get; set; }
    }
}

