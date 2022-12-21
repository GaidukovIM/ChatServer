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
        static Socket serverSocket;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            const string ip="127.0.0.1";
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip),51000);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(10);
            var t = new Thread(new ParameterizedThreadStart(Accept));
            msgs = MSG.GetAllMSGs(jsonFileName);
            foreach (var msg in msgs)
                Console.WriteLine(msg.ToString());
            t.Start(serverSocket.Accept());
        }
        static void Console_CancelKeyPress(object obj,ConsoleCancelEventArgs e)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }
        static void Accept(object obj)
        {
            var listener = (Socket)obj;
            while (true)
            {
                Thread.Sleep(1000);
                StringBuilder builder = Utils.GetStringFromListener(listener);
                if(builder.ToString()=="Get_MSGs")
                {
                    StringBuilder tmp=new StringBuilder();
                    foreach (var msg in msgs)
                        tmp.Append(msg.ToString()+"\n");
                        listener.Send(Encoding.UTF8.GetBytes(tmp.ToString()));
                }
                else
                {
                    if (builder.ToString() != "")
                    {
                        MSG.GetMSG(builder).WriteMsgToFile(jsonFileName);
                        listener.Send(Encoding.UTF8.GetBytes("got_msg"));
                    }
                }
            }
        }
    }
}