using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatLibrary;
using System.Threading;
using System.ComponentModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        const string ip = "127.0.0.1";
        IPEndPoint ClientEndPoint;
        Socket clientSocket;
        List<MSG> msgs;
        public ClientWindow()
        {
            Closing += stop;
            InitializeComponent();
            msgs= new List<MSG>();
            ClientEndPoint = new IPEndPoint(IPAddress.Parse(ip), 51000);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ClientEndPoint);

            var ClientEndPointGet = new IPEndPoint(IPAddress.Parse(ip), 51001);
            var clientSocketGet = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocketGet.Connect(ClientEndPointGet);

            Thread ThreadForGettingMSGs = new Thread(new ParameterizedThreadStart(GettingThread));
            ThreadForGettingMSGs.IsBackground = true;
            ThreadForGettingMSGs.Start(clientSocketGet);
        }
        private void stop(object obj,CancelEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        private void GettingThread(object obj)
        {
            while (true)
            {
                Thread.Sleep(1000);
                var clientSocket = (Socket)obj;
                var listener = clientSocket.Accept();
                int bytesRead;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                do
                {
                    bytesRead = listener.Receive(buffer);
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
                } while (listener.Available > 0);
                TextBoxMSGText.Text=builder.ToString();
            }
        }
        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {            
                string s = $"{DateTime.Now.ToString()} {TextBoxNick.Text}: {TextBoxMSGText.Text}";
                clientSocket.Send(Encoding.Unicode.GetBytes(s));
                TextBoxMSGText.Text = "";
        }
    }
}
