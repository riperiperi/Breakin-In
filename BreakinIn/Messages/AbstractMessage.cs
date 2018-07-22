using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BreakinIn.Messages
{
    public abstract class AbstractMessage
    {
        public abstract string _Name { get; }
        public string PlaintextData = "";

        public void Read(string input)
        {
            if (input.Length < 8) return;
            var type = this.GetType();

            var hasN = input.IndexOf('\n') > 0;
            string[] pairs;
            if (hasN) pairs = input.Split('\n');
            else pairs = input.Split((char)9);

            foreach (var pair in pairs)
            {
                if (pair.Length == 0) continue;
                var eqSplit = pair.Split('=');
                if (eqSplit.Length < 2) continue;
                var prop = type.GetProperty(eqSplit[0]);
                if (prop == null || (prop.PropertyType != typeof(string)) || prop.Name[0] == '_')
                {
                    if (!eqSplit[0].Contains("TIME")) Console.WriteLine("Unexpected Proprerty: " + pair + " (for " + type.ToString() + ")");
                }
                else
                {
                    prop.SetValue(this, eqSplit[1]);
                }
            }
        }

        public string Write()
        {
            var type = this.GetType();
            var props = type.GetProperties();
            var keyValue = new StringBuilder();
            foreach (var prop in props)
            {
                if ((prop.PropertyType != typeof(string) && prop.PropertyType != typeof(string[])) || prop.Name[0] == '_') continue;
                if (prop.PropertyType == typeof(string[]))
                {
                    var values = ((string[])prop.GetValue(this));
                    if (values == null) continue;
                    for (int i=0; i<values.Length; i++)
                    {
                        if (values[i] != null) keyValue.Append(prop.Name + i + "=" + values[i] + '\n');
                    }
                }
                else
                {
                    var value = ((string)prop.GetValue(this));
                    if (value == null) continue;
                    keyValue.Append(prop.Name + "=" + value + '\n');
                }
            }
            if (keyValue.Length == 0) keyValue.Append('\n');
            return keyValue.ToString();
        }

        public virtual void Process(AbstractEAServer context, EAClient client)
        {

        }

        public byte[] GetData()
        {
            var plaintext = (_Name.Length == 8);
            var body = ((plaintext) ? PlaintextData : Write()) + "\0";
            var size = body.Length + 12;

            var mem = new MemoryStream();
            var io = new BinaryWriter(mem);
            io.Write(ASCIIEncoding.ASCII.GetBytes(_Name));
            if (!plaintext)
            {
                io.Write(0); //4 byte pad
            }
            io.Write(new byte[] { (byte)(size >> 24), (byte)(size >> 16), (byte)(size >> 8), (byte)(size) });
            io.Write(ASCIIEncoding.ASCII.GetBytes(body));

            var bytes = mem.ToArray();
            io.Dispose();
            mem.Dispose();
            return bytes;
        }
    }
}
