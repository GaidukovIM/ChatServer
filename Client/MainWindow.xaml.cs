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
        }
        private void stop(object obj,CancelEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
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
        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            string s = $"{DateTime.Now.ToString()} {TextBoxNick.Text} {TextBoxMSGText.Text}";
            clientSocket.Send(Encoding.UTF8.GetBytes(s));
            TextBoxMSGText.Text = "";
            MessageBox.Show(GetStringFromListener(clientSocket).ToString());
        }

        private void ButtonGet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
