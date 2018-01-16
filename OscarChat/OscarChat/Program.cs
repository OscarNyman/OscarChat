using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OscarChat
{
    class Program
    {
        private const int ListenPort = 11000;

        static void Main(string[] args)
        {
            var ListenThread = new Thread(listener);
            ListenThread.Start();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.EnableBroadcast = true;

            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, ListenPort);

            Thread.Sleep(1000);

            while(true)
            {
                Console.Write(">");
                String msg = Console.ReadLine();

                byte[] sendbuf = Encoding.UTF8.GetBytes(msg);
                socket.SendTo(sendbuf, ep);

                Thread.Sleep(200);
            }
        }

        static void listener()
        {
            UdpClient listener = new UdpClient(ListenPort);

            try
            {
                while(true)
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);

                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.WriteLine("Recieved broadcast from {0} : {1}\n", groupEP.ToString(), Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
