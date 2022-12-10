using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatLibrary;
namespace ChatServer
{
    internal class Program
    {
        const string jsonFileName = "MSGs.txt";
        static void Main(string[] args)
        {
            const string ip="127.0.0.1";
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip),51000);
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);
            var t = new Thread(new ParameterizedThreadStart(Accept));
            t.IsBackground = true;
            t.Start((object)serverSocket);
            List<MSG> msgs = MSG.GetAllMSGs(jsonFileName);

        }
        static void Accept(object obj)
        {
            Socket serverSocket = (Socket)obj;
            while(true)
            {
                Thread.Sleep(1000);
                int bytesRead;
                byte[] buffer = new byte[1024];
                StringBuilder builder=new StringBuilder();
                while((bytesRead= serverSocket.Receive(buffer)) > 0)
                {
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
                }
            }
        }
        static void GetMSG(StringBuilder builder)
        {
            string[] tmp = builder.ToString().Split(' ');
            builder.Clear();
            for (int i = 3; i < tmp.Length; i++)
                builder.Append(tmp[i]);
            new MSG(tmp[1]/*Sender*/, builder.ToString()/*Text*/, DateTime.Parse(tmp[0])).WriteMsgToFile(jsonFileName);
        }
    }
}