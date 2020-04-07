using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class ReptOut : AbstractMessage
    {
        public override string _Name { get => "rept"; }

        public string PERS { get; set; }
    }
}