using BreakinIn.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BreakinIn
{
    public abstract class AbstractEAServer : IDisposable
    {
        public abstract Dictionary<string, Type> NameToClass { get; }
        public int SessionID = 1;

        public List<EAClient> Clients = new List<EAClient>();
        public TcpListener Listener;

        private Thread ListenerThread;

        public AbstractEAServer(ushort port)
        {
            IPAddress localAddr = IPAddress.Parse("0.0.0.0");
            Listener = new TcpListener(localAddr, port);
            Listener.Start();

            ListenerThread = new Thread(RunLoop);
            ListenerThread.Start();
        }

        private void RunLoop()
        {
            try
            {
                while (true)
                {
                    //blocks til we get a new connection
                    TcpClient client = Listener.AcceptTcpClient();
                    if (client != null)
                    {
                        var eaC = new EAClient(this, client);
                        eaC.SessionID = SessionID++;
                        AddClient(eaC);
                    }
                }
            }
            catch
            {
                Console.WriteLine("A listener stopped working!");
            }
        }

        public virtual void AddClient(EAClient client)
        {
            lock (Clients)
            {
                Clients.Add(client);
            }
        }

        public virtual void RemoveClient(EAClient client)
        {
            lock (Clients)
            {
                Clients.Remove(client);
            }
        }

        public void Broadcast(AbstractMessage msg)
        {
            var data = msg.GetData();
            lock (Clients)
            {
                foreach (var user in Clients)
                {
                    user.PingSendTick = DateTime.Now.Ticks;
                    user.SendMessage(data);
                }
            }
        }

        public virtual void HandleMessage(string name, byte[] data, EAClient client)
        {
            try
            {
                var body = Encoding.ASCII.GetString(data);

                Type c;
                if (!NameToClass.TryGetValue(name, out c))
                {
                    Console.WriteLine("Unexpected Message Type " + name + ":");
                    Console.WriteLine(body);
                    return;
                }

                var msg = (AbstractMessage)Activator.CreateInstance(c);
                msg.Read(body);

                msg.Process(this, client);
            } catch (Exception)
            {

            }
        }

        public void Dispose()
        {
            
        }
    }
}
