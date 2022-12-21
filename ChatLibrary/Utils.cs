using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public class Utils
    {
        public static StringBuilder GetStringFromListener(Socket listener)
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
    }
}
