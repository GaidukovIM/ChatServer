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
        static Socket serverSocketAcceptingMSGs;
        static Socket serverSocketSendingMSGs;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            const string ip="127.0.0.1";
            var serverEndPointAccept = new IPEndPoint(IPAddress.Parse(ip),51000);
            serverSocketAcceptingMSGs = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocketAcceptingMSGs.Bind(serverEndPointAccept);
            serverSocketAcceptingMSGs.Listen(10);

            var serverEndPointSend = new IPEndPoint(IPAddress.Parse(ip), 51001);
            serverSocketAcceptingMSGs = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocketAcceptingMSGs.Bind(serverEndPointSend);
            serverSocketAcceptingMSGs.Listen(10);

            var gettingThread = new Thread(new ParameterizedThreadStart(Accept));
            gettingThread.Start((object)serverSocketAcceptingMSGs);
            var sendingThread = new Thread(new ParameterizedThreadStart(ThreadSendingMSGs));
            sendingThread.Start((object)serverSocketSendingMSGs);
            msgs = MSG.GetAllMSGs(jsonFileName);
        }
        static void Console_CancelKeyPress(object obj,ConsoleCancelEventArgs e)
        {
            serverSocketAcceptingMSGs.Shutdown(SocketShutdown.Both);
            serverSocketAcceptingMSGs.Close();
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
        static void ThreadSendingMSGs(object obj)
        {
            var serverSocketSendingMSGs=(Socket)obj;
            while (true)
            {
                foreach (var msg in msgs)
                    serverSocketSendingMSGs.Send(Encoding.Unicode.GetBytes(msg.ToString()));
                Thread.Sleep(2000);
            }
        }
        static void Accept(object obj)
        {
            Socket serverSocketAcceptingMSGs = (Socket)obj;
            var listener = serverSocketAcceptingMSGs.Accept();
            while(true)
            {
                Thread.Sleep(1000);
                StringBuilder builder = GetStringFromListener(listener);
                if(builder.Length != 0)
                MSG.GetMSG(builder).WriteMsgToFile(jsonFileName);
            }
        }
    }
}