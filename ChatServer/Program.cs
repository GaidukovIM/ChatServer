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
        static List<MSG> msgs = new List<MSG>();
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
            msgs = MSG.GetAllMSGs(jsonFileName);

        }
        static StringBuilder GetStringFromListener(Socket listener)
        {
            int bytesRead;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((bytesRead = listener.Receive(buffer)) > 0)
            {
                builder.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
            }
            return builder;
        }
        static void Accept(object obj)
        {
            Socket serverSocket = (Socket)obj;
            var listener = serverSocket.Accept();
            while(true)
            {
                Thread.Sleep(1000);
                StringBuilder builder = GetStringFromListener(listener);
                if(builder.ToString()=="New_MSG")
                {
                    listener.Send(Encoding.UTF8.GetBytes("OK"));
                    GetMSG(GetStringFromListener((Socket)listener));
                }
                if(builder.ToString()=="Get_MSGs")
                {
                    foreach (var msg in msgs)
                        listener.Send(Encoding.UTF8.GetBytes(msg.ToString()));
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