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
            t.Start(serverSocket.Accept());
            msgs = MSG.GetAllMSGs(jsonFileName);
        }
        static void Console_CancelKeyPress(object obj,ConsoleCancelEventArgs e)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }
        static StringBuilder GetStringFromListener(Socket listener)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            StringBuilder builder = new StringBuilder();
            do
            {
                bytesRead = listener.Receive(buffer);
                builder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } while (listener.Available > 0);
            return builder;
        }
        static void Accept(object obj)
        {
            var listener = (Socket)obj;
            while (true)
            {
                Thread.Sleep(1000);
                StringBuilder builder = GetStringFromListener(listener);
                if(builder.ToString()=="Get_MSGs")
                {
                    StringBuilder tmp=new StringBuilder();
                    foreach (var msg in msgs)
                        tmp.Append(msg.ToString()+"\n");
                        listener.Send(Encoding.UTF8.GetBytes(tmp.ToString()));
                }
                else
                {
                    //MSG.GetMSG(GetStringFromListener(listener)).WriteMsgToFile(jsonFileName);
                    Console.WriteLine(MSG.GetMSG(builder).ToString());
                    listener.Send(Encoding.UTF8.GetBytes("done"));
                }
            }
        }
    }
}