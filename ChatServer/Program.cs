﻿using System;
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
            t.Start((object)serverSocket);
            msgs = MSG.GetAllMSGs(jsonFileName);
        }
        static void Console_CancelKeyPress(object obj,ConsoleCancelEventArgs e)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
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
                    MSG.GetMSG(GetStringFromListener((Socket)listener)).WriteMsgToFile(jsonFileName); ;
                }
                if(builder.ToString()=="Get_MSGs")
                {
                    foreach (var msg in msgs)
                        listener.Send(Encoding.UTF8.GetBytes(msg.ToString()+"\n"));
                }
            }
        }
    }
}