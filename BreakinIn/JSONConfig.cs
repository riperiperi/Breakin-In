using BreakinIn.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BreakinIn
{
    public class JSONConfig
    {
        private static Config _Inst;
        public static Config Inst
        {
            get
            {
                if (_Inst == null)
                {
                    using (var file = File.Open("config.json", FileMode.Open, FileAccess.Read, FileShare.None))
                    using (var io = new StreamReader(file))
                    {
                        var json = io.ReadToEnd();
                        _Inst = JsonConvert.DeserializeObject<Config>(json);
                    }
                }
                return _Inst;
            }
        }
    }
}
