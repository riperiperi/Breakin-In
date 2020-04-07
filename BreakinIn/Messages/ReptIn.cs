using System;
using System.Collections.Generic;
using System.Text;

namespace BreakinIn.Messages
{
    public class ReptIn : AbstractMessage
    {
        public override string _Name { get => "rept"; }

        public string PERS { get; set; }
        public string PROD { get; set; }
        public string LANG { get; set; }

        public override void Process(AbstractEAServer context, EAClient client)
        {
            var mc = context as MatchmakerServer;
            if (mc == null) return;

            var PlayerReportingPlayer = new ReptOut()
            {
                PERS = PERS,
            };

            var user = client.User;
            if (user == null) return;



            if (user.PersonaName == PERS)
            {
                Console.WriteLine(user.PersonaName + " attempted to report themselves, which is not allowed.");
                client.SendMessage(new ReportingSelf());
            }
            else
            {
                if (user.CurrentRoom.Name.ToString() == null)
                {
                    Console.WriteLine(user.PersonaName + " has reported " + PERS + ". Both players seem to be in a certain room, but it doesn't have a name.");
                    client.SendMessage(PlayerReportingPlayer);
                    return;
                }
                else
                {
                    Console.WriteLine(user.PersonaName + " has reported " + PERS + ". The report has come from " + user.CurrentRoom.Name.ToString() + ".");
                    client.SendMessage(PlayerReportingPlayer);
                    return;
                }
            }
        }
    }
    public class ReportingSelf : AbstractMessage
    {
        public override string _Name { get => "reptself"; }
    }

    public class ReportingDisabled : AbstractMessage
    {
        public override string _Name { get => "reptdisa"; }
    }

    public class ReportedUserHasntSpokenYet : AbstractMessage
    {
        public override string _Name { get => "reptwolf"; }
    }
}