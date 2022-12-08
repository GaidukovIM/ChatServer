using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
namespace ChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip="127.0.0.1";
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip),51000);
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);
            var t = new Thread(new ThreadStart(Accept));
            t.IsBackground = true;
            t.Start();
        }
        static void Accept()
        {
            while(true)
            {
                Thread.Sleep(1000);

            }
        }
    }
}